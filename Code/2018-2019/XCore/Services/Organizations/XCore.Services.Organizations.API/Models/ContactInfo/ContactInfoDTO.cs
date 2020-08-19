using XCore.Services.Organizations.API.Models.Organization;

namespace XCore.Services.Organizations.API.Models.ContactInfo
{
    public class ContactInfoDTO
    {
        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual string Code { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string MetaData { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public virtual string Name { get; set; }
        public virtual string NameCultured { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Fax { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public int OrganizationId { get; set; }
        public virtual OrganizationDTO Organization { get; set; }
    }
}
