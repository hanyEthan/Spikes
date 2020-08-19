using XCore.Services.Personnel.Models.DTO.Essential.Accounts;
using XCore.Services.Personnel.Models.DTO.Organizations;

namespace XCore.Services.Personnel.Models.DTO.Accounts
{
    public class OrganizationAccountDTO : OrganizationAccountEssentialDTO
    {
        public OrganizationDTO Organization { get; set; }
        #region Common
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        #endregion

    }
}
