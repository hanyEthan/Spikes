using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Services.Security.Core.DataLayer.Contracts;

namespace XCore.Services.Security.Core.Contracts
{
   public interface ISecurityDataUnity
    {
        bool? Initialized { get; }
        IActorRepository Actors { get; }
        IRoleRepository Roles { get; }
        ITargetRepository Targets { get; }
        IPrivilegeRepository Privileges { get; }
        IAppsRepository Apps { get; }
        IClaimRepository Claims { get; }

        Task SaveAsync();
    }
}
