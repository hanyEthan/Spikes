using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.Core.Models.Domain
{
    public class Role : Entity<int>
    {
        #region props.

        public virtual int OrganizationId { get; set; } 
        public virtual Organization Organization { get; set; }

        #endregion
        #region cst.
        public Role()
        {
        }
        #endregion
    }
}
