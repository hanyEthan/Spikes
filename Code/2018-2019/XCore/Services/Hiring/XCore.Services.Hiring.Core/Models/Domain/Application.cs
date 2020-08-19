using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.Core.Models.Domain
{
    public class Application : Entity<int>
    {
        #region props
        public virtual int CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }
        public virtual IList<Answer> Answers { get; set; }
        public virtual int AdvertisementId { get; set; }
        public virtual Advertisement Advertisement { get; set; }
        public virtual int HiringStepId { get; set; }
        public virtual HiringStep HiringStep { get; set; }
        public virtual string AppId { get; set; }
        public virtual string ModuleId { get; set; }

        #endregion
        #region cst.
        public Application()
        {
            this.Answers = new List<Answer>();
        }
        #endregion
    }
}
