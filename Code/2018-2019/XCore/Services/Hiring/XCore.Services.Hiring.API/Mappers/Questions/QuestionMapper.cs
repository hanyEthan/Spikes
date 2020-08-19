using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Mappers.Advertisements;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.API.Mappers.Questions
{
    public class QuestionMapper : IModelMapper<Question, QuestionDTO>
    {
        #region props.
        public static QuestionMapper Instance { get; } = new QuestionMapper();

        #endregion

        #region IModelMapper

        public QuestionDTO Map(Question from, object metadata = null)
        {
            if (from == null) return null;

            var to = new QuestionDTO
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
                AdvertisementId = from.AdvertisementId,
                //Advertisement = AdvertisementMapper.Instance.Map(from.Advertisement),
                
            };

            return to;
        }
        public Question Map(QuestionDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Question
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
                AdvertisementId = from.AdvertisementId,
                //Advertisement = AdvertisementMapper.Instance.Map(from.Advertisement),
            };
            return to;
        }

        #endregion
    }
}
