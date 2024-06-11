namespace SMSProvider.Domain;

public class SmsProvider : BaseEntity
{
    public SmsProvider(string providerName, decimal costPerSms, ProviderStatus status)
    {
        ProviderName = providerName;
        CostPerSms = costPerSms;
        CreatedOn = DateTime.UtcNow;
        Status = status;
    }

    public string ProviderName { get; set; }
    public string? ApiDetails { get; set; }
    public ProviderStatus Status { get; set; }
    public decimal CostPerSms { get; set; }
    public int LoadBalanceCounter { get; set; }

    public void Update(string providerName, ProviderStatus status, decimal costPerSms)
    {
        ProviderName = providerName;
        Status = status;
        CostPerSms = costPerSms;
        ModifiedOn = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}

public enum ProviderStatus
{
    Available = 0,
    Busy = 1
}