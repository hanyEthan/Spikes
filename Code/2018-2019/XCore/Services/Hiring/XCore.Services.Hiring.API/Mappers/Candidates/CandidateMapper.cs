using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.API.Mappers.Candidates
{
    public class CandidateMapper : IModelMapper<Candidate, CandidateDTO>
    {
        #region props.

        public static CandidateMapper Instance { get; } = new CandidateMapper();

        #endregion

        #region IModelMapper

        public CandidateDTO Map(Candidate from, object metadata = null)
        {
            if (from == null) return null;

            var to = new CandidateDTO
            {
                AppId = from.AppId,
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = from.CreatedDate,
                Id = from.Id,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = from.ModifiedDate,
                ModuleId = from.ModuleId,
                Name = from.Name,
                NameCultured = from.NameCultured,
                CandidateReferenceId = from.CandidateReferenceId,
            };

            return to;
        }
        public Candidate Map(CandidateDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Candidate
            {
                AppId = from.AppId,
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = from.CreatedDate,
                Id = from.Id,
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = from.ModifiedDate,
                ModuleId = from.ModuleId,
                Name = from.Name,
                NameCultured = from.NameCultured,
                CandidateReferenceId = from.CandidateReferenceId,
            };
            return to;
        }

        #endregion

    }
}
