using System;
using System.Collections.Generic;
using ADS.Common.Models.Domain;
using ADS.Common.Models.Domain.Authorization;
using Action = ADS.Common.Models.Domain.Authorization.Action;

namespace ADS.Common.Contracts.Security
{
    public interface IAuthorizationDataHandler : IBaseHandler
    {
        List<Privilege> GetPrivileges();
        Privilege GetPrivilege(Guid id);
        Privilege AddPrivilege(Privilege item);
        Privilege UpdatePrivilege(Privilege item);
        bool DeletePrivilege(Guid id);

        List<Role> GetRoles();
        List<Role> GetRoles(RoleSearchCriteria criteria);
        Role GetRole(Guid id);
        Role GetRole(Guid id, bool underlyingCollections);
        Role AddRole(Role item);
        Role UpdateRole(Role item);
        bool DeleteRole(Guid id);

        Action GetAction(Guid id);
        Action GetAction(Guid id, bool underlyingCollections);
        List<Action> GetActions();
        Action AddAction(Action item);
        Action UpdateAction(Action item);
        bool DeleteAction(Guid id);

        Actor GetActor(Guid id);
        bool CreateActor( Actor actor );
        bool UpdateActor( Actor actor );

        List<Privilege> GetPrivileges(IAuthorizationActor principle);
        List<Privilege> GetPrivileges(IAuthorizationTarget principle);

        bool Authorize( IAuthorizationActor actor , IAuthorizationTarget target );
        bool Authorize( Guid actorId , string targetName );
    }
}
