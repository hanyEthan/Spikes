using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Mappers.Applications;
using XCore.Services.Hiring.API.Mappers.Questions;
using XCore.Services.Hiring.API.Models.Domain.DTO;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.API.Mappers.Answers
{
    public class AnswerMapper : IModelMapper<Answer, AnswerDTO>
    {
        #region props.

        public static AnswerMapper Instance { get; } = new AnswerMapper();

        #endregion

        #region IModelMapper

        public AnswerDTO Map(Answer from, object metadata = null)
        {
            if (from == null) return null;

            var to = new AnswerDTO
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
                //Application= ApplicationMapper.Instance.Map(from.Application),
                ApplicationId = from.ApplicationId,
                Question = QuestionMapper.Instance.Map(from.Question),
                QuestionId = from.QuestionId,
                
            };

            return to;
        }
        public Answer Map(AnswerDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Answer {
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
                //Application = ApplicationMapper.Instance.Map(from.Application),
                ApplicationId = from.ApplicationId,
                Question = QuestionMapper.Instance.Map(from.Question),
                QuestionId = from.QuestionId,
                
            };
            return to;
        }

        #endregion
    }
}
