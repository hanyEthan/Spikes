using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Framework.Entities.Constants;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Security.API.Model;
using XCore.Services.Security.Core.Models.Domain;

namespace XCore.Services.Security.API.Mappers
{
    public class PrivilegeMapper : IModelMapper<PrivilegeDTO, Privilege>,
                             IModelMapper<Privilege, PrivilegeDTO>
    {
        public static PrivilegeMapper Instance { get; } = new PrivilegeMapper();
        //private ModuleMPrivilegeer ModuleMPrivilegeer { get; set; } = new ModuleMPrivilegeer();

        #region IModelMapper

        public Privilege Map(PrivilegeDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Privilege()
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
                AppId = from.AppId
            };

            return to;
        }
        public PrivilegeDTO Map(Privilege from, object metadata = null)
        {
            if (from == null) return null;

            var to = new PrivilegeDTO()
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
                AppId = from.AppId

            };

            return to;
        }

        #endregion
    }
}
