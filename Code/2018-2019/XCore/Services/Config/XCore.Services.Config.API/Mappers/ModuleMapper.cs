using System.Collections.Generic;
using XCore.Framework.Framework.Entities.Constants;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Config.API.Models;
using XCore.Services.Config.Core.Models.Domain;
using XCore.Services.Config.Core.Models.Support;

namespace XCore.Services.Config.API.Mappers
{
    public class ModuleMapper : IModelMapper<ModuleDTO, Module>, 
                             IModelMapper<Module, ModuleDTO>
    {
        public static ModuleMapper Instance { get; } = new ModuleMapper();
        private ConfigMapper ConfigMapper { get;  } = new ConfigMapper();

        #region IModelMapper

        public Module Map(ModuleDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Module()
            {
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateFormat),
                Description = from.Description,
                IsActive = from.IsActive ?? true,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,
                AppId = from.AppId,
                App = AppMapper.Instance.Map(from.App, metadata = null),
                configlist = Map(from.ConfigList)
                
                
                
               
            };

            return to;
        }
        public ModuleDTO Map(Module from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ModuleDTO()
            {
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateTimeFormat),
                Description = from.Description,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateTimeFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,
                AppId = from.AppId,
                App = AppMapper.Instance.Map(from.App, metadata = null),
                ConfigList = Map(from.configlist)

            };

            return to;
        }

        public List<ConfigDTO> Map(List<ConfigItem> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<ConfigDTO>();

            foreach (var item in from)
            {
                var toItem = this.ConfigMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        public List<ConfigItem> Map(List<ConfigDTO> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<ConfigItem>();

            foreach (var item in from)
            {
                var toItem = this.ConfigMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }


        #endregion
    }
}