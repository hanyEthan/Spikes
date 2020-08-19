using AutoMapper;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Config.Messaging.Processor.IOC.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static IServiceCollection AddAutoMapperSupport(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddAutoMapper(typeof(AutoMapperConfig))
                           .AddSingleton(new MapperConfiguration(cfg =>
                           {
                               cfg.AddProfile<AutoMappingProfile>();
                           }).CreateMapper());
        }
    }
}
