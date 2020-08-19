using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models.Support;

namespace XCore.Services.Organizations.Core.DataLayer.Contracts
{
    public interface ISettingsRepository : IRepository<Settings>
    {
        bool? Initialized { get; }

        Task<bool> AnyAsync(SettingsSearchCriteria criteria);
        Task<SearchResults<Settings>> GetAsync(SettingsSearchCriteria criteria, string includeProperties = null);
    }
}
