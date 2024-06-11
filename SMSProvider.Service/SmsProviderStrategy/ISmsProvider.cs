using SMSProvider.Domain;

namespace SMSProvider.Service.SmsProviderStrategy;

public interface ISmsProvider
{
    Task<bool> SendSmsAsync(string recipient, string message);
    string GetProviderName();
    decimal GetCostPerSms();
    ProviderStatus GetStatus();
}