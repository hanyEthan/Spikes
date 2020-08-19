using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Framework.Entities.Constants;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Security.API.Model;
using XCore.Services.Security.Core.Models.Domain;

namespace XCore.Services.Security.API.MTargeters
{
    public class TargetMapper : IModelMapper<TargetDTO, Target>,
                             IModelMapper<Target, TargetDTO>
    {
        public static TargetMapper Instance { get; } = new TargetMapper();
        //private ModuleMTargeter ModuleMTargeter { get; set; } = new ModuleMTargeter();

        #region IModelMapper

        public Target Map(TargetDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Target()
            {
                Id = from.Id,
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
                PrivilegeId = from.PrivilegeId
                //modulelist = Map(from.modulelist)
            };

            return to;
        }
        public TargetDTO Map(Target from, object metadata = null)
        {
            if (from == null) return null;

            var to = new TargetDTO()
            {
                Id = from.Id,
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
                PrivilegeId = from.PrivilegeId
                // modulelist = Map(from.modulelist)

            };

            return to;
        }

        #endregion
    }
}
