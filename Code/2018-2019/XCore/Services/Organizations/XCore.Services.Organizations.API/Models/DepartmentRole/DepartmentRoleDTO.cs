using XCore.Services.Organizations.API.Models.Department;
using XCore.Services.Organizations.API.Models.Role;

namespace XCore.Services.Organizations.API.Models.DepartmentRole
{
    public class DepartmentRoleDTO
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
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
        public DepartmentDTO DepartmentDTO { get; set; }
        public RoleDTO RoleDTO { get; set; }


    }
}
