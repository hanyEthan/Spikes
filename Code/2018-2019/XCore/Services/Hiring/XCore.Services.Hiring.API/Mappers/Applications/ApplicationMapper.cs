using System.Collections.Generic;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Mappers.Advertisements;
using XCore.Services.Hiring.API.Mappers.Answers;
using XCore.Services.Hiring.API.Mappers.Candidates;
using XCore.Services.Hiring.API.Mappers.HiringSteps;
using XCore.Services.Hiring.API.Models.Domain.DTO;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.API.Mappers.Applications
{
    public class ApplicationMapper : IModelMapper<Application, ApplicationDTO>
    {
        #region props.

        public static ApplicationMapper Instance { get; } = new ApplicationMapper();

        #endregion

        #region IModelMapper

        public ApplicationDTO Map(Application from, object metadata = null)
        {
            if (from == null) return null;

            var to = new ApplicationDTO
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
                HiringStepId = from.HiringStepId,
                CandidateId = from.CandidateId,
                AdvertisementId = from.AdvertisementId,

                Advertisement = AdvertisementMapper.Instance.Map(from.Advertisement),
                Answers = Map(from.Answers),
                Candidate = CandidateMapper.Instance.Map(from.Candidate),
                HiringStep = HiringStepMapper.Instance.Map(from.HiringStep),

            };

            return to;
        }
        public Application Map(ApplicationDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Application
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
                HiringStepId = from.HiringStepId,
                CandidateId = from.CandidateId,
                AdvertisementId = from.AdvertisementId,

                Advertisement = AdvertisementMapper.Instance.Map(from.Advertisement),
                Answers = Map(from.Answers),
                Candidate = CandidateMapper.Instance.Map(from.Candidate),
                HiringStep = HiringStepMapper.Instance.Map(from.HiringStep),
            };
            return to;
        }

        #endregion

        #region helpers.
        #region Answer
        private IList<AnswerDTO> Map(IList<Answer> from)
        {
            if (from == null) return null;

            var to = new List<AnswerDTO>();

            foreach (var fromItem in from)
            {
                var toItem = AnswerMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        private IList<Answer> Map(IList<AnswerDTO> from)
        {
            if (from == null) return null;

            var to = new List<Answer>();

            foreach (var fromItem in from)
            {
                var toItem = AnswerMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }

        #endregion
        #endregion
    }
}
