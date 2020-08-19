using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.API.Models.Domain.DTO;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.API.Models.DTO
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
        public string AppId { get; set; }
        public string ModuleId { get; set; }

        #endregion
        #region cst.
        public ApplicationDTO()
        {
            this.Answers = new List<AnswerDTO>();
        }
        #endregion

    }
}
