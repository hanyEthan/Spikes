using Mcs.Invoicing.Services.Config.Api.Rest.Models;
using Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Commands.CreateConfigItem;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;

namespace Mcs.Invoicing.Services.Config.Api.Rest.Mappers
{
    public class CreateConfigItemCommandDTOMapper : IModelMapper<CreateConfigItemCommandDTO, CreateConfigItemCommand>
    {
        #region props.

        private readonly AutoMapper.IMapper _mapper;

        #endregion
        #region cst.

        public CreateConfigItemCommandDTOMapper(AutoMapper.IMapper mapper)
        {
            this._mapper = mapper;
        }

        #endregion
        #region IModelMapper

        public CreateConfigItemCommand Map(CreateConfigItemCommandDTO from, object metadata = null)
        {
            return _mapper.Map<CreateConfigItemCommand>(from);
        }
        public TDestinationAlt Map<TDestinationAlt>(CreateConfigItemCommandDTO from, object metadata = null) where TDestinationAlt : CreateConfigItemCommand
        {
            return Map(from, metadata) as TDestinationAlt;
        }

        #endregion
    }
}
