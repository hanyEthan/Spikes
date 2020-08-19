using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Domain
{
    public class ContactInfo: Entity<int>
    {
        #region props.

        public string Description { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }

        public int OrganizationId { get; set; }
        public virtual Organization Organization{ get; set; }

        #endregion
    }
}
