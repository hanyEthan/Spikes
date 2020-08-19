using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Mappers;
using Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Commands.CreateConfigItem;

namespace Mcs.Invoicing.Services.Config.Api.Rest.Models
{
    public class CreateConfigItemCommandDTO : IMapFrom<CreateConfigItemCommand>
    {
        #region props.

        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int ModuleId { get; set; }

        #endregion
        #region IMapFrom

        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<CreateConfigItemCommandDTO, CreateConfigItemCommand>();
        }

        #endregion
    }
}
