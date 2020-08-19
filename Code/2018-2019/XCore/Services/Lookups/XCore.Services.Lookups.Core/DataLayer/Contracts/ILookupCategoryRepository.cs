using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Services.Lookups.Core.Models.Domain;

namespace XCore.Services.Lookups.Core.DataLayer.Contracts
{
    public interface ILookupCategoryRepository : IRepository<LookupCategory>
    {
        bool? Initialized { get; }
    }
}
