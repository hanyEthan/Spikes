using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.SDK.Models.DTOs
{
    public class OrganizationDelegationDTO : Entity<int>
    {
        public int DelegateId { get; set; }
        public int DelegatorId { get; set; }
        public virtual int? OrganizationDelegateId { get; set; }
        public virtual OrganizationDTO OrganizationDelegate { get; set; }
        public virtual int? OrganizationDelegatorId { get; set; }
        public virtual OrganizationDTO OrganizationDelegator { get; set; }
    }
}