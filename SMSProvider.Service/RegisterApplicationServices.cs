using Microsoft.Extensions.DependencyInjection;
using SMSProvider.Service.Provider;
using SMSProvider.Service.Sms;
using SMSProvider.Service.SmsProviderStrategy;

namespace SMSProvider.Service;

public static class RegisterApplicationServices
{
    public static void RegisterAppServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ISmsService, SmsService>();
        serviceCollection.AddScoped<ISmsProviderService, SmsProviderService>();

        serviceCollection.AddSingleton<ISmsProvider, TwilioSmsProvider>();
        serviceCollection.AddSingleton<ISmsProvider, VonageSmsProvider>();
    }
}