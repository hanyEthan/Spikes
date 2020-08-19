using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.Core.Models.Relations
{
    public class AdvertisementSkill
    {
        #region props.

        public virtual int AdvertisementId { get; set; }
        public virtual Advertisement Advertisement { get; set; }
        public virtual int SkillId { get; set; }
        public virtual Skill Skill { get; set; }

        #endregion        
    }
}
