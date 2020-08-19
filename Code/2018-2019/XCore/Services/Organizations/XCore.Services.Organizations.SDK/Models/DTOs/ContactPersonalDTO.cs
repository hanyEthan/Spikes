using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.SDK.Models.DTOs
{
    public class ContactPersonalDTO : Entity<int>
    {
        public string Description { get; set; }
        public string PersonName { get; set; }
        public string PersonEmail { get; set; }
        public string PersonMobile { get; set; }
        public string PersonId { get; set; }
        public int? OrganizationId { get; set; }
        public virtual OrganizationDTO Organization { get; set; }

    }
}