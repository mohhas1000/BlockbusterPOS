using BlockBusterPOS.Configuration;

namespace BlockBusterPOS.Extensions;

public static class OpenApiServicesExtensions
{
    public static void AddOpenApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<OpenApiOptions>()
            .Bind(configuration.GetSection(OpenApiOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOpenApiDocument((document, services) =>
        {
            var options = configuration.GetSection(OpenApiOptions.SectionName).Get<OpenApiOptions>() ??
            throw new InvalidOperationException("Failed to retrieve the configured open API section from app settings");

            document.Title = options.Title;
            document.Version = options.Version;
            document.Description = options.Description;
        });

    }
}

