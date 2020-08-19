using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.SDK.Models.DTOs
{
    public class ContactInfoDTO : Entity<int>
    {
        public string Email { get; set; }
        public string Description { get; set; }

        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public int? OrganizationId { get; set; }
        public virtual OrganizationDTO Organization { get; set; }

    }
}