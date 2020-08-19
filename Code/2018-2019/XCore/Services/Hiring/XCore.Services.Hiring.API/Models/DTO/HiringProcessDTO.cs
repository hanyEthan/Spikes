using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.API.Models.DTO
{
    public class HiringProcessDTO : Entity<int>
    {
        #region props.
        public IList<HiringStepDTO> HiringSteps { get; set; }
        public int OrganizationId { get; set; }
        public OrganizationDTO Organization { get; set; }

        #endregion
        #region cst.
        public HiringProcessDTO()
        {
            this.HiringSteps = new List<HiringStepDTO>();
        }
        #endregion
    }
}
