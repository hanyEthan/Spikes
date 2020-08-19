using XCore.Services.Personnel.Models.DTO.Base;

namespace XCore.Services.Personnel.Models.DTO.Essential.Personnels
{
    public class PersonnelEssentialDTO : BaseEntityDTO
    {
        public int? ManagerId { get; set; }
        public int? DepartmentId { get; set; }
    }
}
