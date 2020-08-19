using System.Threading.Tasks;

namespace XCore.Services.Lookups.Core.DataLayer.Contracts
{
    public interface ILookupsDataUnity
    {
        bool? Initialized { get; }
        ILookupRepository Lookups { get; }
        ILookupCategoryRepository LookupCategories { get; }

        Task SaveAsync();
    }
}
