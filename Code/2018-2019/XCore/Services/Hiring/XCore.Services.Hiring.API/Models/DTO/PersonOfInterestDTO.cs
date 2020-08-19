using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.API.Models.DTO
{
    public class PersonOfInterestDTO : Entity<int>
    {
        #region props.
        public int CandidateId { get; set; }
        public CandidateDTO Candidate { get; set; }
        public int OrganizationId { get; set; }
        public OrganizationDTO Organization { get; set; }
        #endregion
        
    }
}
