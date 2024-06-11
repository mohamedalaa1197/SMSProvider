namespace SMSProvider.Domain;

public class SmsLog : BaseEntity
{
    public SmsLog(string messageContent, string recipientPhone, MessageStatus status, string providerUsed)
    {
        MessageContent = messageContent;
        RecipientPhone = recipientPhone;
        Status = status;
        ProviderUsed = providerUsed;
        CreatedOn = DateTime.UtcNow;
    }

    private SmsLog()
    {
    }

    public string MessageContent { get; set; }
    public string RecipientPhone { get; set; }
    public MessageStatus Status { get; set; }
    public string ProviderUsed { get; set; }

    public void Update(string messageContent, string recipient, MessageStatus status, string providerUsed)
    {
        MessageContent = messageContent;
        RecipientPhone = recipient;
        Status = status;
        ProviderUsed = providerUsed;
        ModifiedOn = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}

public enum MessageStatus
{
    Sent = 0,
    Failed = 1,
    Exception = 3
}