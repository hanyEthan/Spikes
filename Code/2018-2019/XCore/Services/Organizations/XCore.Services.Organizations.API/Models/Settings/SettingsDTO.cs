using XCore.Services.Organizations.API.Models.Organization;

namespace XCore.Services.Organizations.API.Models.Settings
{
    public class SettingsDTO
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
        public int OrganizationId { get; set; }
        public virtual OrganizationDTO Organization { get; set; }
    }
}
