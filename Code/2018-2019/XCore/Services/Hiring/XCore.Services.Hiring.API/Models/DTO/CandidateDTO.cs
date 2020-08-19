using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.API.Models.DTO
{
    public class CandidateDTO : Entity<int>
    {
        #region props.

        public string CandidateReferenceId { get; set; }
        public string AppId { get; set; }
        public string ModuleId { get; set; }

        #endregion
        #region cst.
        public CandidateDTO()
        {
        }
        #endregion
    }
}
