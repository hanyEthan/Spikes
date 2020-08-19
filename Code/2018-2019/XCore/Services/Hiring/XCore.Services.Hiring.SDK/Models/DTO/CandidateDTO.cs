using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.SDK.Models.DTO
{
    public class CandidateDTO : Entity<int>
    {
        #region props.

        public string CandidateReferenceId { get; set; }
        public int AppId { get; set; }
        public int ModuleId { get; set; }

        #endregion
        #region cst.
        public CandidateDTO()
        {
        }
        #endregion
    }
}
