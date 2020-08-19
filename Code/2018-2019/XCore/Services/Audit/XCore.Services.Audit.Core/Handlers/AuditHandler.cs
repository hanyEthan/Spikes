using System;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Extensions;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Audit.Core.Contracts;
using XCore.Services.Audit.Core.DataLayer.Contracts;
using XCore.Services.Audit.Core.Models;

namespace XCore.Services.Audit.Core.Handlers
{
    public class AuditHandler : IAuditHandler
    {
        #region props.

        private readonly IAuditDataUnity _DataLayer;
        private readonly IAuditEventsPublisher _EventsPublisher;
        private readonly IModelValidator<AuditTrail> AuditTrailValidator;

        #endregion
        #region cst.

        public AuditHandler(IAuditDataUnity dataLayer, IModelValidator<AuditTrail> auditTrailValidator)
        {
            this._DataLayer = dataLayer;
            this.AuditTrailValidator = auditTrailValidator;
            this.Initialized = Initialize();
        }
        public AuditHandler(IAuditDataUnity dataLayer, IModelValidator<AuditTrail> auditTrailValidator, IAuditEventsPublisher eventsPublisher) : this(dataLayer, auditTrailValidator)
        {
            this._EventsPublisher = eventsPublisher;
        }

        #endregion

        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.AuditHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region IAuditHandler

        public async Task<ExecutionResponse<SearchResults<AuditTrail>>> Get(AuditSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<AuditTrail>>();
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

                var results = await this._DataLayer.AuditRead.GetAsync(criteria);
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
        public async Task<ExecutionResponse<AuditTrail>> Create(AuditTrail auditTrail, RequestContext requestContext)
        {
            var context = new ExecutionContext<AuditTrail>();
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

                await this._DataLayer.Audit.CreateAsync(auditTrail);
                await this._DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, auditTrail);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<AuditTrail>(this.AuditTrailValidator, auditTrail, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }

        #endregion

        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && ( this._DataLayer?.Initialized ?? false );
           // isValid = isValid && ( this._EventsPublisher?.Initialized ?? false );
            isValid = isValid && AuditTrailValidator != null;

            return isValid;
        }

        #endregion
    }
}
