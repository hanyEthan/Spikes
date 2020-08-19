using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Hiring.Core.Models.Domain
{
    public class PersonOfInterest : Entity<int>
    {
        #region props.
        public virtual int CandidateId { get; set; }
        public virtual Candidate Candidate { get; set; }
        public virtual int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        #endregion
        
    }
}
