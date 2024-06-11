using Microsoft.Extensions.Configuration;
using SMSProvider.Domain;
using Vonage;
using Vonage.Request;

namespace SMSProvider.Service.SmsProviderStrategy;

public class VonageSmsProvider : ISmsProvider
{
    private readonly string _apiKey;
    private readonly string _apiSecret;
    private readonly string _brandName;

    public VonageSmsProvider(IConfiguration configuration)
    {
        _brandName = configuration["Vonage:brandName"] ?? throw new Exception("Invalid Vonage ApiKey");
        _apiKey = configuration["Vonage:ApiKey"] ?? throw new Exception("Invalid Vonage ApiKey");
        _apiSecret = configuration["Vonage:ApiSecret"] ?? throw new Exception("Invalid Vonage ApiSecret");
    }

    public async Task<bool> SendSmsAsync(string recipient, string message)
    {
        var credentials = Credentials.FromApiKeyAndSecret(
            _apiKey,
            _apiSecret
        );


        var vonageClient = new VonageClient(credentials);

        var response = await vonageClient.SmsClient.SendAnSmsAsync(new Vonage.Messaging.SendSmsRequest()
        {
            To = recipient,
            From = _brandName,
            Text = message
        });

        return response.Messages[0].Status == "0";
    }

    public string GetProviderName() => "Vonage";
    public decimal GetCostPerSms() => 0.0062m;
    public ProviderStatus GetStatus() => ProviderStatus.Available;
}