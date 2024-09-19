using AutoMapper;

namespace BlockBusterPOS.Extensions;

public static class AutoMapperServicesExtensions
{
    public static void AddAutoMapperServices(this IServiceCollection services)
    {
        var mapperConfiguration = new MapperConfiguration(c =>
        {
            c.AddMaps(typeof(MappingProfiles.TransactionProfile).Assembly);
        });
        mapperConfiguration.AssertConfigurationIsValid();
        mapperConfiguration.CompileMappings();
        services.AddSingleton(mapperConfiguration.CreateMapper());
    }
}
