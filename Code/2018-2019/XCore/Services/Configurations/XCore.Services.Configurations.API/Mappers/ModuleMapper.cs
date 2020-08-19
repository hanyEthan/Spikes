using System.Collections.Generic;
using XCore.Framework.Framework.Entities.Constants;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Configurations.Core.Models.Domain;
using XCore.Services.Configurations.Models;

namespace XCore.Services.Configurations.Mappers
{
    public class ModuleMapper : IModelMapper<ModuleDTO, Module>,
                                IModelMapper<Module, ModuleDTO>
    {
        #region props.

        public static ModuleMapper Instance { get; } = new ModuleMapper();
        private ConfigMapper ConfigMapper { get; } = new ConfigMapper();

        #endregion
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
                Configurations = Map(from.Configurations)




            };

            return to;
        }
        public ModuleDTO Map(Module from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ModuleDTO();

            to.Id = from.Id;
            to.Code = from.Code;
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateTimeFormat);
            to.Description = from.Description;
            to.IsActive = from.IsActive;
            to.MetaData = from.MetaData;
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateTimeFormat);
            to.Name = from.Name;
            to.NameCultured = from.NameCultured;
            to.AppId = from.AppId;
            to.App = AppMapper.Instance.Map(from.App, metadata = null);
            to.Configurations = Map(from.Configurations);

            return to;
        }

        #endregion
        #region helpers.

        public IList<ConfigDTO> Map(IList<ConfigItem> from, object metadata = null)
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
        public IList<ConfigItem> Map(IList<ConfigDTO> from, object metadata = null)
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