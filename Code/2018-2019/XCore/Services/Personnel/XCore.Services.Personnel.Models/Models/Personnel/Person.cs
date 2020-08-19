using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.Models.Departments;

namespace XCore.Services.Personnel.Models.Personnels
{
    public class Person : Entity<int>
    {
        public string AppId { get; set; }
        public string ModuleId { get; set; }
        public int? ManagerId { get; set; }
        public int? DepartmentId { get; set; }
        public virtual Person Manager { get; set; }
        public virtual Department Department { get; set; }
        public virtual PersonnelAccount Account { get; set; }
    }
}
