using System.Net;
using Microsoft.EntityFrameworkCore;
using SMSProvider.Repository.Context;
using SMSProvider.Service.Provider.Models;
using SMSProvider.Shared;

namespace SMSProvider.Service.Provider;

public class SmsProviderService : ISmsProviderService
{
    private readonly SmsProviderDBContext _context;

    public SmsProviderService(SmsProviderDBContext context)
    {
        _context = context;
    }

    public async Task<FuncResponseWithValue<GetProviderResponseModel>> AddProvider(ProviderRequestModel providerRequestModel)
    {
        try
        {
            var isProviderExistsBefore = (await _context.SmsProviders.FirstOrDefaultAsync(x => x.ProviderName.ToLower() == providerRequestModel.ProviderName.ToLower())) is not null;
            if (isProviderExistsBefore)
            {
                return new FuncResponseWithValue<GetProviderResponseModel>(null, HttpStatusCode.BadRequest, ResponseCode.Error, "Provider Already exists");
            }

            var providerEntity = new Domain.SmsProvider(providerRequestModel.ProviderName, providerRequestModel.CostPerSms, providerRequestModel.ProviderStatus);
            _context.SmsProviders.Add(providerEntity);
            await _context.SaveChangesAsync();
            return new FuncResponseWithValue<GetProviderResponseModel>(new GetProviderResponseModel(providerEntity));
        }
        catch (Exception e)
        {
            return new FuncResponseWithValue<GetProviderResponseModel>(new GetProviderResponseModel(null), HttpStatusCode.BadRequest, ResponseCode.Error, "Something went wrong");
        }
    }

    public async Task<PaginatedOutPut<GetProviderResponseModel>> GetProviders(BasePage basePage)
    {
        try
        {
            var providerQuery = _context.SmsProviders.AsQueryable();
            var totalCount = await providerQuery.CountAsync();

            if (basePage.Page.HasValue && basePage.PageSize.HasValue)
            {
                var skip = (basePage.Page.Value - 1) * basePage.PageSize.Value;
                providerQuery = providerQuery.Skip(skip).Take(basePage.PageSize.Value);
            }

            return new PaginatedOutPut<GetProviderResponseModel>(await providerQuery.Select(smsProvider => new GetProviderResponseModel(smsProvider)).ToListAsync(), totalCount, basePage.Page,
                basePage.PageSize);
        }
        catch (Exception e)
        {
            return new PaginatedOutPut<GetProviderResponseModel>(null, 0, basePage.Page,
                basePage.PageSize, ResponseCode.Error);
        }
    }

    public async Task<FuncResponseWithValue<GetProviderResponseModel>> UpdateProvider(int id, ProviderRequestModel providerRequestModel)
    {
        try
        {
            var entity = await _context.SmsProviders.FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null)
            {
                return new FuncResponseWithValue<GetProviderResponseModel>(null, HttpStatusCode.BadRequest, ResponseCode.Error, "Invalid Provider");
            }

            entity.Update(providerRequestModel.ProviderName, providerRequestModel.ProviderStatus, providerRequestModel.CostPerSms);
            await _context.SaveChangesAsync();
            return new FuncResponseWithValue<GetProviderResponseModel>(new GetProviderResponseModel(entity));
        }
        catch (Exception e)
        {
            return new FuncResponseWithValue<GetProviderResponseModel>(new GetProviderResponseModel(null), HttpStatusCode.BadRequest, ResponseCode.Error, "Something went wrong");
        }
    }
}