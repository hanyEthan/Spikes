using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Accounts;

namespace XCore.Services.Personnel.Models.Organizations
{
    public class Organization : Entity<int>
    {
        public string AppId { get; set; }
        public string ModuleId { get; set; }
        public string OrganizationReferenceId { get; set; }
        public virtual OrganizationAccount Account { get; set; }
    }
}
