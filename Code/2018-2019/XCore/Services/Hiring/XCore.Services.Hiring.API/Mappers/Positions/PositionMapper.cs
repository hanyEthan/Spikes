using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Hiring.API.Mappers.Advertisements;
using XCore.Services.Hiring.API.Models.DTO;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.API.Mappers.Positions
{
    public class PositionMapper : IModelMapper<Position, PositionDTO>
    {
        #region props.
        public static PositionMapper Instance { get; } = new PositionMapper();

        #endregion

        #region IModelMapper

        public PositionDTO Map(Position from, object metadata = null)
        {
            if (from == null) return null;

            var to = new PositionDTO
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
        public Position Map(PositionDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Position {
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
