using XCore.Services.Personnel.Models.DTO.Base;

namespace XCore.Services.Personnel.Models.DTO.Essential.Departments
{
    public class DepartmentEssentialDTO : BaseEntityDTO
    {
        public string DepartmentReferenceId { get; set; }
        public int? HeadDepartmentId { get; set; }
    }
}
