using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.Models.Organizations;
using XCore.Services.Personnel.Models.DTO.Organizations;
using XCore.Services.Personnel.Models.DTO.Essential.Organizations;
using XCore.Services.Personnel.API.Mappers.Accounts;

namespace XCore.Services.Personnel.API.Mappers.Organizations
{
    public class OrganizationMapper : OrganizationEssentialMapper<Organization, OrganizationDTO>
    {
        #region props.

        public static OrganizationMapper Instance { get; } = new OrganizationMapper();

        #endregion
        #region cst.

        static OrganizationMapper()
        {
        }
        public OrganizationMapper()
        {
        }

        #endregion

        #region IModelMapper

        public override OrganizationDTO Map(Organization from, object metadata = null)
        {
            if (from == null) return null;
            var to = base.Map(from, metadata);
            to.Account = OrganizationAccountMapper.Instance.Map(from.Account, metadata);
            to.AppId = from.AppId;
            to.ModuleId = from.ModuleId;
            #region Common
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            #endregion
            return to;
        }
        public override Organization Map(OrganizationDTO from, object metadata = null)
        {
            if (from == null) return null;
            var to = base.Map(from, metadata);
            to.Account = OrganizationAccountMapper.Instance.Map(from.Account, metadata);
            to.AppId = from.AppId;
            to.ModuleId = from.ModuleId;
            #region Common
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat);
            #endregion
            return to;
        }
 
        #endregion
    }
}
