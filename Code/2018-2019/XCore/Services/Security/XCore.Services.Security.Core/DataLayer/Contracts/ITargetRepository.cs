using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.Core.DataLayer.Contracts
{
   public interface ITargetRepository : IRepository<Target>
    {
        bool? Initialized { get; }
        Task<bool> AnyAsync(TargetSearchCriteria criteria);
        Task<SearchResults<Target>> GetAsync(TargetSearchCriteria criteria, string includeProperties = null);
    }
}
