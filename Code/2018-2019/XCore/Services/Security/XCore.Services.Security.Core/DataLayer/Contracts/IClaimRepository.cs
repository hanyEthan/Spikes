using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Entities.Repositories.Handlers;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Relations;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.Core.DataLayer.Contracts
{
    public interface IClaimRepository : IRepository<Claim>
    {
        bool? Initialized { get; }
        Task<bool> AnyAsync(ClaimSearchCriteria criteria);
        Task<SearchResults<Claim>> GetAsync(ClaimSearchCriteria criteria, string includeProperties = null);
        void DeletedAssociatedActor(ActorClaim model);
        void DeletedAssociatedRole(RoleClaim model);
    }
}
