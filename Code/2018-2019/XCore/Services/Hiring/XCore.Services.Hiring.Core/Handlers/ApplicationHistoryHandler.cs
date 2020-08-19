using System;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Services.Attachments.Core.DataLayer.Contracts;
using XCore.Services.Hiring.Core.Contracts;
using XCore.Services.Hiring.Core.Models.Domain;
using XCore.Services.Hiring.Core.Models.Search;

namespace XCore.Services.Hiring.Core.Handlers
{
    public class ApplicationHistoryHandler : IApplicationHistoryHandler
    {
        #region props.

        private readonly IHiringDataUnity DataLayer;        

        #endregion
        #region cst.

        public ApplicationHistoryHandler(IHiringDataUnity dataLayer)
        {
            this.DataLayer = dataLayer;            
            this.Initialized = Initialize();
        }

        #endregion

        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.ApplicationHistoryHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region IApplicationHistoryHandler

        public async Task<ExecutionResponse<ApplicationHistory>> Create(ApplicationHistory request, RequestContext requestContext)
        {
            var context = new ExecutionContext<ApplicationHistory>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                if (!this.Initialized.GetValueOrDefault())
                {
                    return context.Response.Set(ResponseState.Error, null);
                }

                #endregion
                #region DL

                await this.DataLayer.ApplicationHistory.CreateAsync(request);
                await this.DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, request);

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
        public async Task<ExecutionResponse<SearchResults<ApplicationHistory>>> Get(ApplicationHistorySearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<ApplicationHistory>>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                if (!this.Initialized.GetValueOrDefault())
                {
                    return context.Response.Set(ResponseState.Error, null);
                }

                #endregion
                #region DL

                var results = await this.DataLayer.ApplicationHistory.Get(criteria);
                return context.Response.Set(ResponseState.Success, results);

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

        #endregion

        #region helpers.

        #region initialize
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (this.DataLayer?.Initialized ?? false);

            return isValid;
        }

        #endregion       

        #endregion
    }
}
