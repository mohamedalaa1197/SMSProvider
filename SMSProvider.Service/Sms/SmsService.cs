using System.Net;
using Microsoft.EntityFrameworkCore;
using SMSProvider.Domain;
using SMSProvider.Repository.Context;
using SMSProvider.Service.SmsProvider.Models;
using SMSProvider.Service.SmsProviderStrategy;
using SMSProvider.Shared;

namespace SMSProvider.Service.Sms
{
    public class SmsService : ISmsService
    {
        private readonly SmsProviderDBContext _context;
        private readonly IEnumerable<ISmsProvider> _smsProviders;
        private readonly RateLimitter.RateLimitter _rateLimiter;

        public SmsService(SmsProviderDBContext context, IEnumerable<ISmsProvider> smsProviders)
        {
            _context = context;
            _smsProviders = smsProviders;
            _rateLimiter = new RateLimitter.RateLimitter(maxAttempts: 3, timeWindow: TimeSpan.FromMinutes(2));
        }

        public async Task<FuncResponseWithValue<bool>> SendSmsAsync(SendSmsRequestModel smsRequestModel)
        {
            const int maxRetries = 3;
            var attempt = 0;

            if (!_rateLimiter.IsAllowed(smsRequestModel.RecipientPhone))
            {
                return new FuncResponseWithValue<bool>(false, HttpStatusCode.TooManyRequests, ResponseCode.Error, "Rate limit exceeded");
            }

            var provider = SelectProvider();
            while (attempt < maxRetries)
            {
                try
                {
                    var result = await provider.SendSmsAsync(smsRequestModel.RecipientPhone, smsRequestModel.MessageBody);
                    var smsLog = new SmsLog(smsRequestModel.MessageBody,smsRequestModel.RecipientPhone, result ? MessageStatus.Sent : MessageStatus.Failed, provider.GetProviderName());
                    _context.SmsLogs.Add(smsLog);
                    await _context.SaveChangesAsync();

                    if (result)
                    {
                        return new FuncResponseWithValue<bool>(true);
                    }
                }
                catch (Exception e)
                {
                    var smsLog = new SmsLog(smsRequestModel.RecipientPhone, smsRequestModel.MessageBody, MessageStatus.Exception, provider.GetProviderName());
                    _context.SmsLogs.Add(smsLog);
                    await _context.SaveChangesAsync();
                    return new FuncResponseWithValue<bool>(false, HttpStatusCode.BadRequest, ResponseCode.Error, "Something went wrong");
                }

                attempt++;
            }

            return new FuncResponseWithValue<bool>(false, HttpStatusCode.BadRequest, ResponseCode.Error, "Error sending Sms message");
        }


        public async Task<PaginatedOutPut<GetSmsLogResponseModel>> GetSmsLogs(BasePage basePage)
        {
            try
            {
                var smsLogQuery = _context.SmsLogs.AsQueryable();

                var totalCount = await smsLogQuery.CountAsync();
                if (basePage.Page.HasValue && basePage.PageSize.HasValue)
                {
                    var skip = (basePage.Page.Value - 1) * basePage.PageSize.Value;
                    smsLogQuery = smsLogQuery.Skip(skip).Take(basePage.PageSize.Value);
                }

                var result = await smsLogQuery.Select(smslog => new GetSmsLogResponseModel()
                {
                    CreatedOn = smslog.CreatedOn,
                    Status = smslog.Status,
                    MessageContent = smslog.MessageContent,
                    ProviderUsed = smslog.ProviderUsed,
                    RecipientPhone = smslog.RecipientPhone
                }).ToListAsync();

                return new PaginatedOutPut<GetSmsLogResponseModel>(result, totalCount, basePage.Page, basePage.PageSize);
            }
            catch (Exception e)
            {
                return new PaginatedOutPut<GetSmsLogResponseModel>(null, 0, basePage.Page, basePage.PageSize, ResponseCode.Error);
            }
        }

        private ISmsProvider SelectProvider()
        {
            var providers = _context.SmsProviders
                .Where(p => p.Status == ProviderStatus.Available)
                .OrderBy(p => p.CostPerSms)
                .ToList();

            if (!providers.Any())
            {
                throw new InvalidOperationException("No SMS providers are available.");
            }

            var selectedProvider = providers
                .OrderBy(p => p.LoadBalanceCounter)
                .First();

            // Increment LoadBalanceCounter for the selected provider
            selectedProvider.LoadBalanceCounter++;
            _context.SmsProviders.Update(selectedProvider);
            _context.SaveChanges();

            return _smsProviders.First(p => p.GetProviderName() == selectedProvider.ProviderName);
        }
    }
}