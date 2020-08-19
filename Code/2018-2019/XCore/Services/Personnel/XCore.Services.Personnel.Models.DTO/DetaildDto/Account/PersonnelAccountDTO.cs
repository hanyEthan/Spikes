using XCore.Services.Personnel.Models.DTO.Essential.Accounts;
using XCore.Services.Personnel.Models.DTO.Personnels;

namespace XCore.Services.Personnel.Models.DTO.Accounts
{
    public class PersonnelAccountDTO : PersonnelAccountEssentialDTO
    {
        public PersonnelDTO Person { get; set; }
        #region Common
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; } 
        #endregion

    }
}
