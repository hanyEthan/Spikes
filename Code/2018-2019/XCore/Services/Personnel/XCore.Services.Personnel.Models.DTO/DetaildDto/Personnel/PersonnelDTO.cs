using XCore.Services.Personnel.Models.DTO.Essential.Personnels;
using XCore.Services.Personnel.Models.DTO.Departments;
using XCore.Services.Personnel.Models.DTO.Accounts;

namespace XCore.Services.Personnel.Models.DTO.Personnels
{
    public class PersonnelDTO : PersonnelEssentialDTO
    {
        public string AppId { get; set; }
        public string ModuleId { get; set; }
        public PersonnelDTO Manager { get; set; }
        public DepartmentDTO Department { get; set; }
        public PersonnelAccountDTO Account { get; set; }
        #region Common
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        #endregion
    }
}
