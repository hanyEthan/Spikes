using Mcs.Invoicing.Services.Config.Api.gRPC.Protos;
using Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Commands.CreateConfigItem;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;

namespace Mcs.Invoicing.Services.Config.Api.gRPC.Mappers
{
    public class CreateConfigItemCommandProtoMapper : IModelMapper<CreateConfigItemCommandProto, CreateConfigItemCommand>
    {
        #region IModelMapper

        public CreateConfigItemCommand Map(CreateConfigItemCommandProto from, object metadata = null)
        {
            if (from == null) return null;

            var to = new CreateConfigItemCommand()
            {
                Key = from.Key,
                Description = from.Description,
                ModuleId = from.ModuleId,
                Value = from.Value,
            };

            return to;

        }
        public TDestinationAlt Map<TDestinationAlt>(CreateConfigItemCommandProto from, object metadata = null) where TDestinationAlt : CreateConfigItemCommand
        {
            return Map(from, metadata) as TDestinationAlt;
        }

        #endregion
    }
}
