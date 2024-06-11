using SMSProvider.Domain;

namespace SMSProvider.Service.Provider.Models;

public class ProviderRequestModel
{
    public string ProviderName { get; set; }
    public decimal CostPerSms { get; set; }
    public ProviderStatus ProviderStatus { get; set; }
}