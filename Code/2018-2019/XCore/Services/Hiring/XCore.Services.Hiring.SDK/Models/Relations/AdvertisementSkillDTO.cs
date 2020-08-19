using XCore.Services.Hiring.SDK.Models.DTO;

namespace XCore.Services.Hiring.SDK.Models.Relations
{
    public class AdvertisementSkillDTO
    {
        #region props.

        public virtual int AdvertisementId { get; set; }
        public virtual AdvertisementDTO Advertisement { get; set; }
        public virtual int SkillId { get; set; }
        public virtual SkillDTO Skill { get; set; }

        #endregion        
    }
}
