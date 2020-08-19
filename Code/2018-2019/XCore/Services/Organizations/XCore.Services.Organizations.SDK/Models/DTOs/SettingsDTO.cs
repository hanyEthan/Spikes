using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.SDK.Models.DTOs
{
    public class SettingsDTO : Entity<int>
    {
        public string Description { get; set; }
        public int? OrganizationId { get; set; }
        public virtual OrganizationDTO Organization { get; set; }


    }
}