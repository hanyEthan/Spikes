using System.Collections.Generic;
using XCore.Services.Personnel.Models.DTO.Essential.Departments;
using XCore.Services.Personnel.Models.DTO.Personnels;

namespace XCore.Services.Personnel.Models.DTO.Departments
{
    public class DepartmentDTO : DepartmentEssentialDTO
    {
        public DepartmentDTO HeadDepartment { get; set; }
        public string AppId { get; set; }
        public string ModuleId { get; set; }

        #region Common
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        #endregion
    }
}
