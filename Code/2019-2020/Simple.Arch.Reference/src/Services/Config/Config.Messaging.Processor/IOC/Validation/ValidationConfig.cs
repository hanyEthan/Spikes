using FluentValidation.AspNetCore;
using Mcs.Invoicing.Services.Config.Application.Common.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Config.Messaging.Processor.IOC.Validation
{
    public static class ValidationConfig
    {
        public static IMvcBuilder AddValidation(this IMvcBuilder mvcBuilder)
        {
            return mvcBuilder.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<IConfigurationDataUnity>());
        }
    }
}
