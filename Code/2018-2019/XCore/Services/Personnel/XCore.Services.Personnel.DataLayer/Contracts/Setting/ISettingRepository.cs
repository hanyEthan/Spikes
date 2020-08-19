using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Personnel.Models.Settings;

namespace XCore.Services.Personnel.DataLayer.Contracts.Settings
{
    public interface ISettingRepository : IRepository<Setting>
    {
        bool? Initialized { get; }
        Task<bool> AnyAsync(SettingSearchCriteria criteria);
        Task<SearchResults<Setting>> GetAsync(SettingSearchCriteria criteria, string includeProperties = null);
    }
}
