using WazeCredit.Utilities.AppSettingsClasses;

namespace WazeCredit.Utilities.DI;

public static class DIAppSettingsConfig
{
    public static IServiceCollection AddAppSettingsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
        services.Configure<WazeForecastSettings>(configuration.GetSection("WazeForecast"));
        services.Configure<SendGridSettings>(configuration.GetSection("SendGrid"));
        services.Configure<TwilioSettings>(configuration.GetSection("Twilio"));

        return services;
    }
}