using SMSProvider.Domain;

namespace SMSProvider.Service.Provider.Models;

public class GetProviderResponseModel
{
    public GetProviderResponseModel(Domain.SmsProvider smsProvider)
    {
        if (smsProvider is null)
        {
            return;
        }

        ProviderName = smsProvider.ProviderName;
        CostPerSms = smsProvider.CostPerSms;
        NumberOfUses = smsProvider.LoadBalanceCounter;
        Status = smsProvider.Status;
    }

    public string ProviderName { get; set; }
    public decimal CostPerSms { get; set; }
    public int NumberOfUses { get; set; }
    public ProviderStatus Status { get; set; }
}