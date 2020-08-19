using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.SDK.Models.DTO
{
    public class OrganizationDTO : Entity<int>
    {
        #region props.
        public int AppId { get; set; }
        public int ModuleId { get; set; }
        public string OrganizationReferenceId { get; set; }
        public IList<HiringProcessDTO> HiringProcesses { get; set; }
        public IList<RoleDTO> Roles { get; set; }
        
        #endregion
        #region cst.
        public OrganizationDTO()
        {
            this.HiringProcesses = new List<HiringProcessDTO>();
            this.Roles = new List<RoleDTO>();
        } 
        #endregion
    }
}
