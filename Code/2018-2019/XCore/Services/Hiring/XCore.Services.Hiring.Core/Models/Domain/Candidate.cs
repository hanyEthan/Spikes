using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.Core.Models.Domain
{
    public class Candidate : Entity<int>
    {
        #region props.

        public virtual string CandidateReferenceId { get; set; }
        public virtual string AppId { get; set; }
        public virtual string ModuleId { get; set; }

        #endregion
        #region cst.
        public Candidate()
        {
        }
        #endregion
    }
}
