namespace SMSProvider.Service.SmsProvider.Models;

public class SendSmsRequestModel
{
    public string RecipientPhone { get; set; }
    public string MessageBody { get; set; }
}