using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.SDK.Models.DTO
{
    public class ApplicationDTO : Entity<int>
    {
        #region props
        public int CandidateId { get; set; }
        public CandidateDTO Candidate { get; set; }
        public IList<AnswerDTO> Answers { get; set; }
        public int AdvertisementId { get; set; }
        public AdvertisementDTO Advertisement { get; set; }
        public int HiringStepId { get; set; }
        public HiringStepDTO HiringStep { get; set; }
        public int AppId { get; set; }
        public int ModuleId { get; set; }

        #endregion
        #region cst.
        public ApplicationDTO()
        {
            this.Answers = new List<AnswerDTO>();
        }
        #endregion

    }
}
