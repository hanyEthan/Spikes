using System;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Personnel.API.Mappers.Organizations;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.Models.DTO.Accounts;
using XCore.Services.Personnel.Models.DTO.Essential.Accounts;

namespace XCore.Services.Personnel.API.Mappers.Accounts
{
    public class OrganizationAccountMapper : OrganizationAccountEssentialMapper<OrganizationAccount, OrganizationAccountDTO>
    {
        #region props.

        public static OrganizationAccountMapper Instance { get; } = new OrganizationAccountMapper();

        #endregion
        #region cst.

        static OrganizationAccountMapper()
        {
        }
        public OrganizationAccountMapper()
        {
        }

        #endregion

        #region IModelMapper

        public override OrganizationAccountDTO Map(OrganizationAccount from, object metadata = null)
        {
            if (from == null) return null;
            var to = base.Map(from, metadata);
            to.Organization = OrganizationMapper.Instance.Map(from.Organization, metadata);
            #region Common
            to.CreatedBy = from.CreatedBy;
            to.CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat);
            to.ModifiedBy = from.ModifiedBy;
            to.ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat); 
            #endregion
            return to;
        }

        public override OrganizationAccount Map(OrganizationAccountDTO from, object metadata = null)
        {
            if (from == null) return null;
            var to = base.Map(from, metadata);
            to.Organization = OrganizationMapper.Instance.Map(from.Organization, metadata);
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
