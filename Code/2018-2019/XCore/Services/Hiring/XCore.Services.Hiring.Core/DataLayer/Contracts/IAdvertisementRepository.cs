using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.DataLayer.Contracts
{
    public interface IAdvertisementRepository : IRepository<Advertisement>
    {
        bool? Initialized { get; }
        Task<SearchResults<Advertisement>> Get(AdvertisementsSearchCriteria criteria, string includeProperties = null);
        Task<bool> AnyAsync(AdvertisementsSearchCriteria criteria);

    }
}