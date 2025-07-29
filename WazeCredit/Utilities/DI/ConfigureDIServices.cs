using Microsoft.Extensions.DependencyInjection.Extensions;
using WazeCredit.Data.Repository;
using WazeCredit.Models;
using WazeCredit.Service;

namespace WazeCredit.Utilities.DI;

public static class ConfigureDISettings
{
    public static IServiceCollection AddAllServices(this IServiceCollection services)
    {
        services.AddTransient<IMarketForecaster, MarketForecaster>();
        //services.AddScoped<IValidationChecker, AddressValidationChecker>()
        //services.AddScoped<IValidationChecker, CreditValidationChecker>()
        services.AddScoped<ICreditValidator, CreditValidator>();
        services.AddTransient<TransientService>();
        services.AddScoped<ScopedService>();
        services.AddSingleton<SingletonService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<CreditApprovedHigh>();
        services.AddScoped<CreditApprovedLow>();
        services.AddScoped<Func<CreditApprovedEnum, ICreditApproved>>(ServiceProvider => range =>
        {
            switch (range)
            {
                case CreditApprovedEnum.High:
                    return ServiceProvider.GetService<CreditApprovedHigh>();
                case CreditApprovedEnum.Low:
                    return ServiceProvider.GetService<CreditApprovedLow>();
                default:
                    return ServiceProvider.GetService<CreditApprovedLow>();

            }
        });

        // services.TryAddEnumerable(ServiceDescriptor.Scoped<IValidationChecker, AddressValidationChecker>());
        // services.TryAddEnumerable(ServiceDescriptor.Scoped<IValidationChecker, CreditValidationChecker>());

        services.TryAddEnumerable(
            [
                ServiceDescriptor.Scoped<IValidationChecker, AddressValidationChecker>(),
                ServiceDescriptor.Scoped<IValidationChecker, CreditValidationChecker>()
            ]
        );

        return services;
    }
}