using XCore.Framework.Infrastructure.Entities.Repositories.Models;

namespace XCore.Services.Personnel.Models.Departments
{
    public class Department : Entity<int>
    {
        public string DepartmentReferenceId { get; set; }
        public int? HeadDepartmentId { get; set; }
        public string AppId { get; set; }
        public string ModuleId { get; set; }

        
        public virtual Department HeadDepartment { get; set; }
    }
}
