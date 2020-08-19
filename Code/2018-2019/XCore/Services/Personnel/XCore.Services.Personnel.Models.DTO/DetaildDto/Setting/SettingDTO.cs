using XCore.Services.Personnel.Models.DTO.Essential.Settings;

namespace XCore.Services.Personnel.Models.DTO.Settings
{

    public class SettingDTO : SettingEssentialDTO
    {
        #region Common
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        #endregion
    }
}
