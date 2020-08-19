using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Services.Organizations.API.Models.Organization;

namespace XCore.Services.Organizations.API.Models.OrganizationDelegation
{
    public class OrganizationDelegationSearchCriteriaDTO
    {     
        public int? OrganizationDelegateId { get; set; }
        public int? OrganizationDelegatorId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; } = true;

        public bool PagingEnabled { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public int? Order { get; set; }
        public int? OrderByDirection { get; set; }
        public int? OrderByCultureMode { get; set; }
    }
}
