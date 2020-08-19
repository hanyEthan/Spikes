using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Services.Lookups.Core.DataLayer.Context;
using XCore.Services.Lookups.Core.DataLayer.Contracts;
using XCore.Services.Lookups.Core.Models.Domain;

namespace XCore.Services.Lookups.Core.DataLayer.Repositories
{
    public class LookupCategoryRepository : Repository<LookupCategory>, ILookupCategoryRepository
    {
        #region props.

        public bool? Initialized { get; protected set; }

        #endregion
        #region cst.

        public LookupCategoryRepository(LookupsDataContext dataContext) : base(dataContext)
        {
            this.Initialized = Initialize();
        }

        #endregion

        #region ILookupCategoryRepository


        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && base.context != null;

            return isValid;
        }

        #endregion
    }
}
