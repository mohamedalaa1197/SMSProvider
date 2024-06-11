using SMSProvider.Domain;

namespace SMSProvider.Service.SmsProvider.Models;

public class GetSmsLogResponseModel
{
    public string MessageContent { get; set; }
    public string RecipientPhone { get; set; }
    public MessageStatus Status { get; set; }
    public string ProviderUsed { get; set; }
    public DateTime CreatedOn { get; set; }
}