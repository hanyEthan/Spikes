using System;
using System.Collections.Generic;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.API.Models.Relations;

namespace XCore.Services.Hiring.API.Models.DTO
{
    public class AdvertisementDTO : Entity<int>
    {
        #region props.

        public IList<PositionDTO> Positions { get; set; }
        public IList<AdvertisementSkillDTO> Skills { get; set; }
        public IList<QuestionDTO> Questions { get; set; }
        public int HiringProccesId { get; set; }
        public HiringProcessDTO HiringProcces { get; set; }
        public int RoleId { get; set; }
        public RoleDTO Role { get; set; }
        public int OrganizationId { get; set; }
        public OrganizationDTO Organization { get; set; }  
        public string AppId { get; set; }
        public string ModuleId { get; set; }
        public string Title { get; set; }

        #endregion
        #region cst.

        public AdvertisementDTO()
        {
            this.Positions = new List<PositionDTO>();
            this.Questions = new List<QuestionDTO>();
            this.Skills = new List<AdvertisementSkillDTO>();
        }        
        #endregion
    }
}
