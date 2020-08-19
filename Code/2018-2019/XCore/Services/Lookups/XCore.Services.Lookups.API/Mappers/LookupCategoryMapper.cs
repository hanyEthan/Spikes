using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Framework.Entities.Constants;
using XCore.Framework.Framework.Entities.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Contracts;
using XCore.Services.Lookups.API.Models;
using XCore.Services.Lookups.Core.Models.Domain;

namespace XCore.Services.Lookups.API.Mappers
{
    public class LookupCategoryMapper : IModelMapper<LookupCategoryDTO, LookupCategory>,
                                        IModelMapper<LookupCategory, LookupCategoryDTO>
    {
        #region props.

        public static LookupCategoryMapper Instance { get; } = new LookupCategoryMapper();

        #endregion
        #region IModelMapper

        public LookupCategory Map(LookupCategoryDTO from, object metadata = null)
        {
            if (from == null) return null;

            var to = new LookupCategory()
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
                Lookups = Map(from.Lookups),
            };

            return to;
        }
        public LookupCategoryDTO Map(LookupCategory from, object metadata = null)
        {
            if (from == null) return null;

            var to = new LookupCategoryDTO()
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
                Lookups = Map(from.Lookups),
            };

            return to;
        }

        #endregion
        #region helpers.

        private List<LookupDTO> Map(IList<Lookup> from)
        {
            if (from == null) return null;

            var to = new List<LookupDTO>();
            foreach (var fromItem in from)
            {
                var toItem = LookupMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }
        private IList<Lookup> Map(List<LookupDTO> from)
        {
            if (from == null) return null;

            var to = new List<Lookup>();
            foreach (var fromItem in from)
            {
                var toItem = LookupMapper.Instance.Map(fromItem);
                if (toItem != null) to.Add(toItem);
            }

            return to;
        }

        #endregion
    }
}
