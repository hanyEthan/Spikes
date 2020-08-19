using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.Core.Models.Domain
{
    public class HiringProcess : Entity<int>
    {
        #region props.
        public virtual IList<HiringStep> HiringSteps { get; set; }
        public virtual int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        #endregion
        #region cst.
        public HiringProcess()
        {
            this.HiringSteps = new List<HiringStep>();
        }
        #endregion
    }
}
