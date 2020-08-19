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
    public class TargetHandler : ITargetHandler
    {
        #region props.

        private string Target_DataInclude_Full { get; set; }
        private string Target_DataInclude_Basic { get; set; }

        private readonly ISecurityDataUnity _DataHandler;


        #endregion
        #region cst.

        public TargetHandler(ISecurityDataUnity dataHandler)
        {

            this._DataHandler = dataHandler;
            this.Initialized = this.Initialize();
        }

        #endregion
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.SecurityHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region Target

        public async Task<ExecutionResponse<SearchResults<Target>>> Get(TargetSearchCriteria criteria, RequestContext requestContext)
        {
            try
            {
                var context = new ExecutionContext<SearchResults<Target>>();
                await context.Process(async () =>
                {
                    #region Logic
                    #region check.

                    Check();

                    #endregion
                    #region DL


                    #region mode.

                    string includes = criteria.InquiryMode == InquiryMode.Basic ? this.Target_DataInclude_Basic
                                    : criteria.InquiryMode == InquiryMode.Search ? this.Target_DataInclude_Basic
                                    : criteria.InquiryMode == InquiryMode.Full ? this.Target_DataInclude_Full
                                    : null;

                    #endregion

                    var Targets = await _DataHandler.Targets.GetAsync(criteria, includes);
                    return context.Response.Set(ResponseState.Success, Targets);


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
        public async Task<ExecutionResponse<bool>> IsExists(TargetSearchCriteria criteria, RequestContext requestContext)
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


                    var isExisting = await _DataHandler.Targets.AnyAsync(criteria);
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

                #region Target : basic

                this.Target_DataInclude_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region Target : full

                this.Target_DataInclude_Full = string.Join(",", new List<string>()
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
