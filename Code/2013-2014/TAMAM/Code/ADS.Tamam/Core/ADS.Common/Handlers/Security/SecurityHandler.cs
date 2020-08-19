using System;
using System.Collections.Generic;
using System.Linq;
using ADS.Common.Models.Domain;
using ADS.Common.Models.Domain.Authorization;
using ADS.Common.Utilities;
using ADS.Common.Contracts.Security;
using ADS.Common.Validation;
using ADS.Common.Validators;
using Action = ADS.Common.Models.Domain.Authorization.Action;
using ADS.Common.Handlers.Cache;

namespace ADS.Common.Handlers.Security
{
    /// <summary>
    /// KFS default custom security handler, providing authentication and authorization support
    /// </summary>
    public class SecurityHandler : IAuthorizationHandler
    {
        #region Properties

        public bool Initialized { get; private set; }
        public string Name { get { return "SecurityHandler"; } }

        //private IAuthenticationDataHandler _AuthenticationDataHandler;
        //public IAuthenticationDataHandler AuthenticationDataHandler
        //{
        //    set
        //    {
        //        _AuthenticationDataHandler = value;
        //        UpdateState();
        //    }
        //}

        private IAuthorizationDataHandler _AuthorizationDataHandler;
        public IAuthorizationDataHandler AuthorizationDataHandler
        {
            set
            {
                _AuthorizationDataHandler = value;
                UpdateState();
            }
        }

        #endregion
        #region cst.

        public SecurityHandler()
        {
            XLogger.Info("SecurityHandler ...");

            try
            {
                Initialized = false; // waiting for the datastores to be set ...
            }
            catch (Exception x)
            {
                XLogger.Error("SecurityHandler ... Exception: " + x);
            }
        }

        #endregion
        # region IAuthorizationHandler

        # region Privileges

        public List<Privilege> GetPrivileges()
        {
            #region Cache

            //var cacheKey = "SecurityHandler_GetPrivileges";
            //var cached = Broker.Cache.Get<List<Privilege>>( CacheClusters.Security , cacheKey );
            //if ( cached != null ) return cached;

            #endregion

            var privileges = _AuthorizationDataHandler.GetPrivileges();

            #region Cache

            //Broker.Cache.Add<List<Privilege>>( CacheClusters.Security , cacheKey , privileges );

            #endregion

            return privileges;
        }
        public List<Privilege> GetPrivileges( IAuthorizationActor principle )
        {
            #region Cache

            //var cacheKey = "SecurityHandler_GetPrivileges" + principle.Id;
            //var cached = Broker.Cache.Get<List<Privilege>>( CacheClusters.Security , cacheKey );
            //if ( cached != null ) return cached;

            #endregion

            var privileges = _AuthorizationDataHandler.GetPrivileges( principle );

            #region Cache

            //Broker.Cache.Add<List<Privilege>>( CacheClusters.Security , cacheKey , privileges );

            #endregion

            return privileges;
        }
        public List<Privilege> GetPrivileges( IAuthorizationTarget principle )
        {
            #region Cache

            //var cacheKey = "SecurityHandler_GetPrivileges" + principle.Id;
            //var cached = Broker.Cache.Get<List<Privilege>>( CacheClusters.Security , cacheKey );
            //if ( cached != null ) return cached;

            #endregion

            var privileges = _AuthorizationDataHandler.GetPrivileges( principle );

            #region Cache

            //Broker.Cache.Add<List<Privilege>>( CacheClusters.Security , cacheKey , privileges );

            #endregion

            return privileges;
        }
        public Privilege GetPrivilege( Guid id )
        {
            #region Cache

            //var cacheKey = "SecurityHandler_GetPrivilege" + id;
            //var cached = Broker.Cache.Get<Privilege>( CacheClusters.Security , cacheKey );
            //if ( cached != null ) return cached;

            #endregion

            var privilege = _AuthorizationDataHandler.GetPrivilege( id );

            #region Cache

            //Broker.Cache.Add<Privilege>( CacheClusters.Security , cacheKey , privilege );

            #endregion

            return privilege;
        }
        public Privilege AddPrivilege( Privilege item )
        {
            var privilegeUpdated = _AuthorizationDataHandler.AddPrivilege( item );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Security );

            #endregion

            return privilegeUpdated;
        }
        public Privilege UpdatePrivilege( Privilege item )
        {
            var privilegeUpdated = _AuthorizationDataHandler.UpdatePrivilege( item );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Security );

            #endregion

            return privilegeUpdated;
        }
        public bool DeletePrivilege( Guid id )
        {
            var state = _AuthorizationDataHandler.DeletePrivilege( id );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Security );

            #endregion

            return state;
        }

        # endregion
        # region Roles

        public List<Role> GetRoles()
        {
            #region Cache

            //var cacheKey = "SecurityHandler_GetRoles";
            //var cached = Broker.Cache.Get<List<Role>>( CacheClusters.Security , cacheKey );
            //if ( cached != null ) return cached;

            #endregion

            var roles = _AuthorizationDataHandler.GetRoles();

            #region Cache

            //Broker.Cache.Add<List<Role>>( CacheClusters.Security , cacheKey , roles );

            #endregion

            return roles;
        }
        public List<Role> GetRoles( RoleSearchCriteria criteria )
        {
            #region Cache

            //var cacheKey = "SecurityHandler_GetRoles" + criteria;
            //var cached = Broker.Cache.Get<List<Role>>( CacheClusters.Security , cacheKey );
            //if ( cached != null ) return cached;

            #endregion

            var roles = _AuthorizationDataHandler.GetRoles( criteria );

            #region Cache

            //Broker.Cache.Add<List<Role>>( CacheClusters.Security , cacheKey , roles );

            #endregion

            return roles;
        }
        public Role GetRole( Guid id )
        {
            #region Cache

            //var cacheKey = "SecurityHandler_GetRole" + id;
            //var cached = Broker.Cache.Get<Role>( CacheClusters.Security , cacheKey );
            //if ( cached != null ) return cached;

            #endregion

            var role = _AuthorizationDataHandler.GetRole( id );

            #region Cache

            //Broker.Cache.Add<Role>( CacheClusters.Security , cacheKey , role );

            #endregion

            return role;
        }
        public Role GetRole( Guid id , bool underlyingCollections )
        {
            #region Cache

            //var cacheKey = "SecurityHandler_GetRole" + id + underlyingCollections;
            //var cached = Broker.Cache.Get<Role>( CacheClusters.Security , cacheKey );
            //if ( cached != null ) return cached;

            #endregion

            var role = _AuthorizationDataHandler.GetRole( id , underlyingCollections );

            #region Cache

            //Broker.Cache.Add<Role>( CacheClusters.Security , cacheKey , role );

            #endregion

            return role;
        }
        public Role AddRole( Role item )
        {
            #region validation

            var validaionErrors = new List<ModelMetaPair>();
            var validator = new RoleValidator( item , RoleValidator.ValidationMode.Create );
            if ( !validator.IsValid.Value )
            {
                validaionErrors.AddRange( validator.ErrorsDetailed );
                return null;
            }
            
            #endregion
            
            var roleUpdated = _AuthorizationDataHandler.AddRole( item );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Security );

            #endregion

            return roleUpdated;
        }
        public Role UpdateRole( Role item )
        {
            #region validation

            var validaionErrors = new List<ModelMetaPair>();
            var validator = new RoleValidator( item , RoleValidator.ValidationMode.Edit );
            if ( !validator.IsValid.Value )
            {
                validaionErrors.AddRange( validator.ErrorsDetailed );
                return null;
            }
            
            #endregion

            var roleUpdated = _AuthorizationDataHandler.UpdateRole( item );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Security );

            #endregion

            return roleUpdated;
        }
        public bool DeleteRole( Guid id )
        {
            var role = this.GetRole( id , underlyingCollections: true );
            if ( role == null || role.Privileges.Any() ) return false;
            
            // SystemRole cannot be deleted..
            if ( role.SystemRole ) return false;

            var state = _AuthorizationDataHandler.DeleteRole( id );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Security );

            #endregion
            
            return state;
        }

        # endregion
        # region Actions

        public Action GetAction( Guid id )
        {
            #region Cache

            //var cacheKey = "SecurityHandler_GetAction" + id;
            //var cached = Broker.Cache.Get<Action>( CacheClusters.Security , cacheKey );
            //if ( cached != null ) return cached;

            #endregion

            var action = _AuthorizationDataHandler.GetAction( id );

            #region Cache

            //Broker.Cache.Add<Action>( CacheClusters.Security , cacheKey , action );

            #endregion

            return action;
        }
        public Action GetAction( Guid id , bool underlyingCollections )
        {
            #region Cache

            //var cacheKey = "SecurityHandler_GetAction" + id + underlyingCollections;
            //var cached = Broker.Cache.Get<Action>( CacheClusters.Security , cacheKey );
            //if ( cached != null ) return cached;

            #endregion

            var action = _AuthorizationDataHandler.GetAction( id , underlyingCollections );

            #region Cache

            //Broker.Cache.Add<Action>( CacheClusters.Security , cacheKey , action );

            #endregion

            return action;
        }
        public List<Action> GetActions()
        {
            #region Cache

            //var cacheKey = "SecurityHandler_GetActions";
            //var cached = Broker.Cache.Get<List<Action>>( CacheClusters.Security , cacheKey );
            //if ( cached != null ) return cached;

            #endregion

            var actions = _AuthorizationDataHandler.GetActions();

            #region Cache

            //Broker.Cache.Add<List<Action>>( CacheClusters.Security , cacheKey , actions );

            #endregion

            return actions;
        }
        public Action AddAction( Action item )
        {
            var actionUpdated = _AuthorizationDataHandler.AddAction( item );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Security );

            #endregion

            return actionUpdated;
        }
        public Action UpdateAction( Action item )
        {
            var actionUpdated = _AuthorizationDataHandler.UpdateAction( item );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Security );

            #endregion

            return actionUpdated;
        }
        public bool DeleteAction( Guid id )
        {
            var state = _AuthorizationDataHandler.DeleteAction( id );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Security );

            #endregion

            return state;
        }

        # endregion
        # region Authorize

        public bool Authorize( IAuthorizationActor actor , IAuthorizationTarget target )
        {
            return _AuthorizationDataHandler.Authorize( actor , target );
        }
        public bool Authorize( Guid actorId , string targetName )
        {
            return _AuthorizationDataHandler.Authorize( actorId , targetName );
        }

        # endregion
        # region Actor

        public Actor GetActor( Guid id )
        {
            #region Cache

            //var cacheKey = "SecurityHandler_GetActor" + id;
            //var cached = Broker.Cache.Get<Actor>( CacheClusters.Security , cacheKey );
            //if ( cached != null ) return cached;

            #endregion

            var actor = _AuthorizationDataHandler.GetActor( id );

            #region Cache

            //Broker.Cache.Add<Actor>( CacheClusters.Security , cacheKey , actor );

            #endregion

            return actor;
        }
        public bool CreateActor( Actor actor )
        {
            var state = _AuthorizationDataHandler.CreateActor( actor );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Security );

            #endregion

            return state;
        }
        public bool UpdateActor( Actor actor )
        {
            var state = _AuthorizationDataHandler.UpdateActor( actor );

            #region Cache

            Broker.Cache.Invalidate( CacheClusters.Security );

            #endregion

            return state;
        }

        # endregion

        # endregion
        #region Helpers

        private bool UpdateState()
        {
            //bool authenticationLayerState = _AuthenticationDataHandler == null ? false : _AuthenticationDataHandler.Initialized;
            bool authorizationLayerState = _AuthorizationDataHandler == null ? false : _AuthorizationDataHandler.Initialized;

            //return Initialized = authenticationLayerState && authorizationLayerState;
            return Initialized = authorizationLayerState;
        }

        #endregion
    }
}