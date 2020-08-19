using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Organizations.API.Models.OrganizationDelegation;
using XCore.Services.Organizations.Core.Models.Domain;

namespace XCore.Services.Organizations.API.Mappers
{
    public class OrganizationDelegationMapper : IModelMapper<OrganizationDelegationDTO, OrganizationDelegation>,
                             IModelMapper<OrganizationDelegation, OrganizationDelegationDTO>
    {
        #region props.

        public static OrganizationDelegationMapper Instance { get; } = new OrganizationDelegationMapper();
        public static OrganizationMapper OrganizationMapper { get; } = new OrganizationMapper();


        #endregion
        #region IModelMapper

        public OrganizationDelegation Map(OrganizationDelegationDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new OrganizationDelegation()
            {
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat),
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,
                Id = from.Id,
               DelegateId=(int)from.OrganizationDelegateId,
               DelegatorId=(int)from.OrganizationDelegatorId

            };

            return to;
        }
        public OrganizationDelegationDTO Map(OrganizationDelegation from, object metadata = null)
        {
            if (from == null) return null;

            var to = new OrganizationDelegationDTO()
            {

                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.Formats.DateTimeFormat),
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.Formats.DateTimeFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,
                Id = from.Id,
                OrganizationDelegateId = from.DelegateId,
                OrganizationDelegatorId = from.DelegatorId

            };

            return to;
        }

        #endregion

    }
}
