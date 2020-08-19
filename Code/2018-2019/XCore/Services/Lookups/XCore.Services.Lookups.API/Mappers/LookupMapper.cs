using XCore.Framework.Framework.Entities.Constants;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Lookups.API.Models;
using XCore.Services.Lookups.Core.Models.Domain;

namespace XCore.Services.Lookups.API.Mappers
{
    public class LookupMapper : IModelMapper<LookupDTO, Lookup>,
                                IModelMapper<Lookup, LookupDTO>
    {
        #region props.

        public static LookupMapper Instance { get; } = new LookupMapper();

        #endregion
        #region IModelMapper

        public Lookup Map(LookupDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new Lookup()
            {
                Id = from.Id,
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateFormat),
                IsActive = from.IsActive ?? true,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,
                CategoryId = from.CategoryId,
            };

            return to;
        }
        public LookupDTO Map(Lookup from, object metadata = null)
        {
            if (from == null) return null;

            var to = new LookupDTO()
            {
                Id = from.Id,
                Code = from.Code,
                CreatedBy = from.CreatedBy,
                CreatedDate = DateMapper.Instance.Map(from.CreatedDate, XCoreConstants.DateTimeFormat),
                IsActive = from.IsActive,
                MetaData = from.MetaData,
                ModifiedBy = from.ModifiedBy,
                ModifiedDate = DateMapper.Instance.Map(from.ModifiedDate, XCoreConstants.DateTimeFormat),
                Name = from.Name,
                NameCultured = from.NameCultured,
                CategoryId = from.CategoryId,
            };

            return to;
        }

        #endregion
    }
}
