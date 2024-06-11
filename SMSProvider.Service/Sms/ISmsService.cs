using SMSProvider.Service.SmsProvider.Models;
using SMSProvider.Shared;

namespace SMSProvider.Service.Sms
{
    public interface ISmsService
    {
        Task<FuncResponseWithValue<bool>> SendSmsAsync(SendSmsRequestModel smsRequestModel);
        Task<PaginatedOutPut<GetSmsLogResponseModel>> GetSmsLogs(BasePage basePage);
    }
}