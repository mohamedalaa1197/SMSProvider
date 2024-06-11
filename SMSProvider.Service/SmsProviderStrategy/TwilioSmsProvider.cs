using Microsoft.Extensions.Configuration;
using SMSProvider.Domain;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SMSProvider.Service.SmsProviderStrategy;

public class TwilioSmsProvider : ISmsProvider
{
    private readonly string _accountSid;
    private readonly string _authToken;
    private readonly string _fromNumber;


    public TwilioSmsProvider(IConfiguration configuration)
    {
        _accountSid = configuration["Twilio:AccountSid"];
        _authToken = configuration["Twilio:AuthToken"];
        _fromNumber = configuration["Twilio:FromNumber"];
    }

    public async Task<bool> SendSmsAsync(string recipient, string message)
    {
        TwilioClient.Init(_accountSid, _authToken);

        var messageResponse = await MessageResource.CreateAsync(
            body: message,
            from: new Twilio.Types.PhoneNumber(_fromNumber),
            to: new Twilio.Types.PhoneNumber(recipient)
        );

        return messageResponse.Status == MessageResource.StatusEnum.Queued;
    }


    public string GetProviderName() => "Twilio";
    public decimal GetCostPerSms() => 0.2777m;
    public ProviderStatus GetStatus() => ProviderStatus.Available;
}