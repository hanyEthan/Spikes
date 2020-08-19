using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Domain
{
    public class OrganizationDelegation : Entity<int>
    {
        #region Conventions
        
        public virtual int DelegateId { get; set; }
        public virtual Organization Delegate { get; set; }
        
        public virtual int DelegatorId { get; set; }
        public virtual Organization Delegator { get; set; }
        
        #endregion
    }
}
