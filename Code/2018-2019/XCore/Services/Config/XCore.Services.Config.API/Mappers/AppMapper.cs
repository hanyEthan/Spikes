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
    public class AppMapper : IModelMapper<AppDTO, App>, 
                             IModelMapper<App, AppDTO>
    {

        public static AppMapper Instance { get; } = new AppMapper();
        private ModuleMapper ModuleMapper { get; set; } = new ModuleMapper();


        #region IModelMapper

        public App Map(AppDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new App()
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
                modulelist = Map(from.modulelist)
            };

            return to;
        }
        public AppDTO Map(App from, object metadata = null)
        {
            if (from == null) return null;

            var to = new AppDTO()
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
                modulelist = Map(from.modulelist)

            };

            return to;
        }

        public List<ModuleDTO> Map(List<Module> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<ModuleDTO>();

            foreach (var item in from)
            {
                var toItem = this.ModuleMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        public List<Module> Map(List<ModuleDTO> from, object metadata = null)
        {
            if (from == null) return null;

            var to = new List<Module>();

            foreach (var item in from)
            {
                var toItem = this.ModuleMapper.Map(item);
                if (toItem == null) return null;

                to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
