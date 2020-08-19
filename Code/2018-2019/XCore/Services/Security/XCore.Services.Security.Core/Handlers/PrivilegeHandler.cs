using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Utilities;
using XCore.Services.Security.Core.Contracts;
using XCore.Services.Security.Core.Models.Domain;
using XCore.Services.Security.Core.Models.Support;

namespace XCore.Services.Security.Core.Handlers
{
   public class PrivilegeHandler : IPrivilegeHandler
    {
        #region props.

        private string Privilege_DataInclude_Full { get; set; }
        private string Privilege_DataInclude_Basic { get; set; }

        private readonly ISecurityDataUnity _DataHandler;


        #endregion
        #region cst.

        public PrivilegeHandler(ISecurityDataUnity dataHandler)
        {

            this._DataHandler = dataHandler;
            this.Initialized = this.Initialize();
        }

        #endregion
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.SecurityHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region Privilege

        public async Task<ExecutionResponse<SearchResults<Privilege>>> Get(PrivilegeSearchCriteria criteria, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<SearchResults<Privilege>>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL


                    #region mode.

                    string includes = criteria.InquiryMode == InquiryMode.Basic ? this.Privilege_DataInclude_Basic
                                    : criteria.InquiryMode == InquiryMode.Search ? this.Privilege_DataInclude_Basic
                                    : criteria.InquiryMode == InquiryMode.Full ? this.Privilege_DataInclude_Full
                                    : null;

                    #endregion

                    var privileges = await _DataHandler.Privileges.GetAsync(criteria, includes);
                    return context.Response.Set(ResponseState.Success, privileges);


                    #endregion

                    #endregion
                }
                #region context

                , new ActionContext()
                {
                    Request = requestContext,
                });
                return context.Response;

                #endregion
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
            
        }
        public async Task<ExecutionResponse<bool>> IsExists(PrivilegeSearchCriteria criteria, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<bool>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL


                    var isExisting = await _DataHandler.Privileges.AnyAsync(criteria);
                    return context.Response.Set(ResponseState.Success, isExisting);


                    #endregion

                    #endregion
                }
                #region context

                 , new ActionContext()
                 {
                     Request = requestContext,
                 });
                return context.Response;

                #endregion
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                throw;
            }
           
        }

        #endregion
        #region Helpers
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && InitializeIncludes();

            isValid = isValid && (this._DataHandler?.Initialized ?? false);
            return isValid;
        }
        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("not initialized correctly.");
            }
        }
        private bool InitializeIncludes()
        {
            try
            {

                #region Privilege : basic

                this.Privilege_DataInclude_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Privilege : full

                this.Privilege_DataInclude_Full = string.Join(",", new List<string>()
                {
                    "App",
                    "Roles",
                    "Actors",
                    "Target"
                });

                #endregion

                return true;
            }
            catch (Exception x)
            {
                XLogger.Error($"Exception : {x}");
                return false;
            }
        }
        #endregion
    }
}
