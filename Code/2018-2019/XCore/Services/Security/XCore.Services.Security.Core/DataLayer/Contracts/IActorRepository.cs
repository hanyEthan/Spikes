﻿using System;
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
   public interface IActorRepository : IRepository<Actor>
    {
        bool? Initialized { get; }
        Task<bool> AnyAsync(ActorSearchCriteria criteria);
        Task<SearchResults<Actor>> GetAsync(ActorSearchCriteria criteria, string includeProperties = null);
        void DeletedAssociatedPrivilege(ActorPrivilege model);
        void DeletedAssociatedRole(ActorRole model);


    }
}
