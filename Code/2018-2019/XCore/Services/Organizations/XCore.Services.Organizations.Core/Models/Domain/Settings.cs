using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Organizations.Core.Models.Domain
{
    public class Settings : Entity<int>
    {
        #region props.

        public string Description { get; set; }

        public int OrganizationId { get; set; }
        public Organization Organization { get; set; }

        #endregion
    }
}
