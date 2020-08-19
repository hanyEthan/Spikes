using System.Threading.Tasks;
using XCore.Services.Lookups.Core.DataLayer.Context;
using XCore.Services.Lookups.Core.DataLayer.Contracts;

namespace XCore.Services.Lookups.Core.DataLayer.Unity
{
    public class LookupsDataUnity : ILookupsDataUnity
    {
        #region props

        public bool? Initialized { get; protected set; }
        private readonly LookupsDataContext _DataContext;

        public ILookupCategoryRepository LookupCategories { get; private set; }
        public ILookupRepository Lookups { get; private set; }

        #endregion
        #region cst.

        public LookupsDataUnity(LookupsDataContext dataContext,
                                ILookupCategoryRepository lookupCategoryRepository,
                                ILookupRepository lookupRepository)
        {
            this._DataContext = dataContext;
            this.LookupCategories = lookupCategoryRepository;
            this.Lookups = lookupRepository;

            this.Initialized = Initialize();
        }

        #endregion

        #region publics

        public async Task SaveAsync()
        {
            await this._DataContext.SaveChangesAsync();
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this._DataContext != null;
            isValid = isValid && (this.Lookups?.Initialized.GetValueOrDefault() ?? false);
            isValid = isValid && (this.LookupCategories?.Initialized.GetValueOrDefault() ?? false);

            return isValid;
        }

        #endregion
    }
}
