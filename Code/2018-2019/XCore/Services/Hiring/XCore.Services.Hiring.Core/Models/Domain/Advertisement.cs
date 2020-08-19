using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models.Relations;

namespace XCore.Services.Hiring.Core.Models.Domain
{
    public class Advertisement : Entity<int>
    {
        #region props.

        public virtual IList<Position> Positions { get; set; }
        public virtual IList<AdvertisementSkill> Skills { get; set; }
        public virtual IList<Question> Questions { get; set; }
        public virtual int HiringProccesId { get; set; }
        public virtual HiringProcess HiringProcces { get; set; }
        public virtual int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual int OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }  
        public virtual string AppId { get; set; }
        public virtual string ModuleId { get; set; }
        public virtual string Title { get; set; }

        #endregion
        #region cst.

        public Advertisement()
        {
            this.Positions = new List<Position>();
            this.Questions = new List<Question>();
            this.Skills = new List<AdvertisementSkill>();
        }

        #endregion
    }
}
