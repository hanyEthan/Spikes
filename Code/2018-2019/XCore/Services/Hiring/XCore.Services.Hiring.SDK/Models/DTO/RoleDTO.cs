using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.SDK.Models.DTO
{
    public class RoleDTO : Entity<int>
    {
        #region props.

        public int OrganizationId { get; set; } 
        public OrganizationDTO Organization { get; set; }

        #endregion
        #region cst.
        public RoleDTO()
        {
        }
        #endregion
    }
}
