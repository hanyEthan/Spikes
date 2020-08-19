using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Domain
{
    public class ContactPerson: Entity<int>
    {
        #region Props.

        public string Description { get; set; }
        public string PersonEmail { get; set; }
        public string PersonMobile { get; set; }
        public string PersonReferenceId { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }

        #endregion
    }
}
