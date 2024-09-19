using BlockBusterPOS.Configuration;
using BlockBusterPOS.Interfaces.Repositories;
using BlockBusterPOS.Interfaces.Services;
using BlockBusterPOS.Repositories;
using BlockBusterPOS.Services;

namespace BlockBusterPOS.Extensions;

public static class TransactionConfigurationExtensions
{
    public static void RegisterTransactionServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<CustomerTransactionOptions>()
            .Bind(configuration.GetSection(CustomerTransactionOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<RentalPriceOptions>()
            .Bind(configuration.GetSection(RentalPriceOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddScoped<ITransactionService, TransactionService>();
        services.AddSingleton<ITransactionRepository, TransactionRepository>();
    }
}

