using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Components.Framework.Context.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Utilities;

namespace XCore.Framework.Infrastructure.Context.Execution.Extensions
{
    [Serializable]
    public class AuthorizationContext : IContextStep
    {
        #region props.

        public List<string> ActionKeys { get; set; }
        private IActionContext Context;
        private BulkAuthorizationMode BulkMode;

        #endregion
        #region cst.

        public AuthorizationContext() { }
        public AuthorizationContext( string actionKey ) : this()
        {
            this.ActionKeys = new List<string>() { actionKey };
        }
        public AuthorizationContext( List<string> actionKeys , BulkAuthorizationMode bulkMode ) : this()
        {
            this.ActionKeys = actionKeys;
            this.BulkMode = bulkMode;
        }

        #endregion
        #region IContextStep

        public async Task<IResponse> Process( IActionContext context )
        {
            this.Context = context;

            return await Auth();
        }

        #endregion

        #region helpers

        private async Task<ExecutionResponse<SecurityContextBase>> Auth()
        {
            var response = new ExecutionResponse<SecurityContextBase>() { State = ResponseState.Success , };

            #region authorize

            bool authorizedForAllPrivileges = true;

            foreach ( var actionKey in this.ActionKeys )
            {
                bool authorized = await Authorize( this.Context.Request , actionKey );

                if ( !authorized && this.BulkMode == BulkAuthorizationMode.All )
                {
                    if ( this.BulkMode == BulkAuthorizationMode.All )
                    {
                        response.State = ResponseState.AccessDenied;
                        return response;
                    }
                    else
                    {
                        authorizedForAllPrivileges = false;
                    }
                }
            }

            if ( !authorizedForAllPrivileges )
            {
                response.State = ResponseState.AccessDenied;
                return response;
            }

            #endregion
            #region authenticate

            var state = await Authenticate(this.Context.Request);
            if ( !state)
            {
                response.State = ResponseState.AuthenticationError;
                return response;
            }

            #endregion

            return response;
        }
        private static async Task<bool> Authenticate( RequestContext context )
        {
            //return Broker.Broker.System.AuthenticationService.Authenticate( context.SecurityToken );
            return true;
        }
        private static async Task<bool> Authorize( RequestContext context , string target )
        {
            try
            {
                #region Validate

                if ( context is SystemRequestContext ) return true;
                if ( context == null || string.IsNullOrEmpty( context.UserId ) || string.IsNullOrEmpty( target ) ) return false;

                #endregion
                #region Cache

                //var cacheKey = "TamamServiceBroker_Authorize" + context + target;
                //var cached = Broker.Cache.Get<bool?>( TamamCacheClusters.Security , cacheKey );
                //if ( cached != null ) return cached.Value;

                #endregion

                //bool isAuthorized = SysUnity.Security.Authorize( context.UserId , target );

                #region Cache

                //Broker.Cache.Add<bool?>( TamamCacheClusters.Security , cacheKey , isAuthorized );

                #endregion

                //return isAuthorized;
                return false;
            }
            catch ( Exception x )
            {
                XLogger.Error( "Exception : " + x );
                return false;
            }
        }

        #endregion
        #region nested.

        public enum BulkAuthorizationMode
        {
            All = 0,
            Some = 1,
        }

        #endregion
    }
}
