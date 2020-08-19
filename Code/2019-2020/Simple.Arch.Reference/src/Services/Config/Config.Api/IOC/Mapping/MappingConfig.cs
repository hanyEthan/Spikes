using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Config.Api;
using Mcs.Invoicing.Core.Framework.Infrastructure.Context.Mappers;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Mappers;
using Mcs.Invoicing.Services.Config.Api.gRPC.Mappers;
using Mcs.Invoicing.Services.Config.Api.gRPC.Protos;
using Mcs.Invoicing.Services.Config.Api.Rest.Mappers;
using Mcs.Invoicing.Services.Config.Api.Rest.Models;
using Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Commands.CreateConfigItem;
using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mcs.Invoicing.Services.Config.Api.IOC.Mapping
{
    public static class MappingConfig
    {
        public static IServiceCollection AddMapping(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddAutoMapper(typeof(Startup))
                           .AddSingleton(new MapperConfiguration(cfg =>
                           {
                               cfg.AddProfile<AutoMappingProfile>();
                           }).CreateMapper())

                           .AddSingleton<IModelMapper<BaseRequestContext, BaseRequestContext>, RequestContextMapper>()

                           .AddSingleton<IModelMapper<SearchResults<ConfigItem>, SearchResults<ConfigItemDTO>>, ConfigItemListMapper>()
                           .AddSingleton<IModelMapper<ConfigItem, ConfigItemDTO>, ConfigItemMapper>()
                           .AddSingleton<IModelMapper<CreateConfigItemCommandDTO, CreateConfigItemCommand>, CreateConfigItemCommandDTOMapper>()

                           .AddSingleton<IModelMapper<CreateConfigItemCommandProto, CreateConfigItemCommand>, CreateConfigItemCommandProtoMapper>()
                           .AddSingleton<IModelMapper<BaseResponseContext<int>, CreateConfigItemResponseProto>, CreateConfigItemResponseProtoMapper>();
        }
    }
}
