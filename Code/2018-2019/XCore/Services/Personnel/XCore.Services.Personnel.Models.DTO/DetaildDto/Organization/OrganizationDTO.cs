using XCore.Services.Personnel.Models.DTO.Essential.Organizations;
using XCore.Services.Personnel.Models.DTO.Accounts;

namespace XCore.Services.Personnel.Models.DTO.Organizations
{
    public class OrganizationDTO : OrganizationEssentialDTO
    {
        public string AppId { get; set; }
        public string ModuleId { get; set; }
        public OrganizationAccountDTO Account { get; set; }
        #region Common
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        #endregion

    }
}
