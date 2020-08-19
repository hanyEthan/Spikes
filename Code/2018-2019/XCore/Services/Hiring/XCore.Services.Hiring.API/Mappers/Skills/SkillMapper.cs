using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.API.Mappers.Skills
{
    public class SkillMapper : IModelMapper<Skill, SkillDTO>
    {
        #region props.

        public static SkillMapper Instance { get; } = new SkillMapper();

        #endregion

        #region IModelMapper

        public SkillDTO Map(Skill from, object metadata = null)
        {
            if (from == null) return null;

            var to = new SkillDTO
            {
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = from.CreatedDate,
                Id = from.Id,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = from.ModifiedDate,
                Name = from.Name,
                NameCultured = from.NameCultured,

                
            };

            return to;
        }
        public Skill Map(SkillDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Skill
            {
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = from.CreatedDate,
                Id = from.Id,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = from.ModifiedDate,
                Name = from.Name,
                NameCultured = from.NameCultured,

            };
            return to;
        }

        #endregion

    }
}
