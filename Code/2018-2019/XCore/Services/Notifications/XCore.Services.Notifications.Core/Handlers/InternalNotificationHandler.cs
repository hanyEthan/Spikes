
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Extensions;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Framework.Utilities;
using XCore.Services.Docs.SDK.Contracts;
using XCore.Services.Notifications.Core.Contracts;
using XCore.Services.Notifications.Core.Models.Domain;
using XCore.Services.Notifications.Core.Models.Support;

namespace XCore.Services.Notifications.Core.Handlers
{
    public class InternalNotificationHandler : IInternalNotificationHandler
    {
        #region prop.
        private string MessageTemplate_DataInclude_Basic { get; set; }
        private string MessageTemplate_DataInclude_Search { get; set; }
        private string MessageTemplate_DataInclude_Full { get; set; }

        private readonly IDocumentClient DocumentsService;
        private readonly INotificationsDataUnity _DataHandler;
        private readonly IModelValidator<InternalNotification> InternalNotificationValidators;
        #endregion
        #region cst.
        public InternalNotificationHandler(INotificationsDataUnity dataHandler, IModelValidator<InternalNotification> InternalNotificationValidators, IDocumentClient DocumentClient)
        {
            this.InternalNotificationValidators = InternalNotificationValidators;
            this._DataHandler = dataHandler;
            this.DocumentsService = DocumentClient;

            this.Initialized = this.Initialize();
        }

        #endregion
        #region IUnityService
        public bool? Initialized { get; protected set; }

        public string ServiceId { get { return $"XCore.InternalNotificationHandler.{Guid.NewGuid()}"; } }
        #endregion
        #region Actions
        
        public async Task<ExecutionResponse<InternalNotification>> Create(InternalNotification InternalNotification, RequestContext requestContext)
        {
            var context = new ExecutionContext<InternalNotification>();
            await context.Process(async () =>
            {
                #region Logic
                #region DL.

                await _DataHandler.InternalNotification.CreateAsync(InternalNotification);
                await _DataHandler.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, InternalNotification);

                #endregion
            }
            #region context

             , new ActionContext()
             {
                 Request = requestContext,
                Validation = new ValidationContext<InternalNotification>(this.InternalNotificationValidators, InternalNotification, ValidationMode.Create),
             });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await _DataHandler.InternalNotification.GetFirstAsync(x => x.Id == id, null, this.MessageTemplate_DataInclude_Full);
                if (existing == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }

                #region validation.

                var validationResponse = await this.InternalNotificationValidators.ValidateAsync(existing, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                _DataHandler.InternalNotification.Delete(existing);
                await _DataHandler.SaveAsync();

                #endregion
             
                return context.Response.Set(ResponseState.Success, true);

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
        public async Task<ExecutionResponse<bool>> Delete(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await _DataHandler.InternalNotification.GetFirstAsync(x => x.Code == code);
                if (existing == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }

                #region validation.

                var validationResponse = await this.InternalNotificationValidators.ValidateAsync(existing, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                _DataHandler.InternalNotification.Delete(existing);
                await _DataHandler.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, true);

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
        public async Task<ExecutionResponse<SearchResults<InternalNotification>>> Get(InternalNotificationSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<InternalNotification>>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                #region mode.

                string includes = criteria.InquiryMode == InquiryMode.Basic ? this.MessageTemplate_DataInclude_Basic
                                : criteria.InquiryMode == InquiryMode.Search ? this.MessageTemplate_DataInclude_Search
                                : criteria.InquiryMode == InquiryMode.Full ? this.MessageTemplate_DataInclude_Full
                                : null;

                #endregion

                var IntenalNotification = await _DataHandler.InternalNotification.GetAsync(criteria, includes);
                return context.Response.Set(ResponseState.Success, IntenalNotification);

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
        public async Task<ExecutionResponse<bool>> MarkasDismissed(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await _DataHandler.InternalNotification.GetFirstAsync(x => x.Id == id);
                if (existing == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }

                #region validation.
                var validationResponse = await this.InternalNotificationValidators.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await _DataHandler.InternalNotification.SetIsDismissedAsync(existing, true);
                await _DataHandler.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, true);

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
        public async Task<ExecutionResponse<bool>> MarkasDismissed(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await _DataHandler.InternalNotification.GetFirstAsync(x => x.Code == code);
                if (existing == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }

                #region validation.
                var validationResponse = await this.InternalNotificationValidators.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await _DataHandler.InternalNotification.SetIsDismissedAsync(existing, false);
                await _DataHandler.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, true);

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
        public async Task<ExecutionResponse<bool>> MarkasDeleted(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await _DataHandler.InternalNotification.GetFirstAsync(x => x.Id == id);
                if (existing == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }

                #region validation.
                var validationResponse = await this.InternalNotificationValidators.ValidateAsync(existing, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await _DataHandler.InternalNotification.SetIsDeletedAsync(existing, true);
                await _DataHandler.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, true);

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
        public async Task<ExecutionResponse<bool>> MarkasDeleted(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await _DataHandler.InternalNotification.GetFirstAsync(x => x.Code == code);
                if (existing == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }

                #region validation.
                var validationResponse = await this.InternalNotificationValidators.ValidateAsync(existing, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await _DataHandler.InternalNotification.SetIsDeletedAsync(existing, false);
                await _DataHandler.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, true);

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
        public async Task<ExecutionResponse<bool>> MarkasRead(List<int?> id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await _DataHandler.InternalNotification.GetAsync( new InternalNotificationSearchCriteria() { Id = id });
                if (existing == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }

                #region validation.
                
                foreach (var doc in existing.Results)
                {
                    var validationResponse = await this.InternalNotificationValidators.ValidateAsync(doc, ValidationMode.Edit);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }
                }

              
                #endregion

                await _DataHandler.InternalNotification.SetIsReadAsync(existing.Results,true);
                await _DataHandler.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, true);

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
        public async Task<ExecutionResponse<bool>> MarkasRead(List<string> code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await _DataHandler.InternalNotification.GetAsync(new InternalNotificationSearchCriteria() { Code = code });
                if (existing == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }

                #region validation.

                foreach (var doc in existing.Results)
                {
                    var validationResponse = await this.InternalNotificationValidators.ValidateAsync(doc, ValidationMode.Edit);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }
                }


                #endregion

                await _DataHandler.InternalNotification.SetIsReadAsync(existing.Results, false);
                await _DataHandler.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, true);

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
        public async Task<ExecutionResponse<bool>> IsUnique(InternalNotification MessageTemplate, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await _DataHandler.MessageTemplate.AnyAsync(x =>(x.Code != MessageTemplate.Code) &&(x.IsActive == true));

                return context.Response.Set(ResponseState.Success, !isExisting);

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
        public async Task<ExecutionResponse<bool>> IsExists(InternalNotificationSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await _DataHandler.InternalNotification.AnyAsync(criteria);
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
        #endregion
        #region helpers.
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && InitializeIncludes();

            isValid = isValid && (this._DataHandler?.Initialized ?? false);
            isValid = isValid && InternalNotificationValidators != null;

            return isValid;
        }
        private bool InitializeIncludes()
        {
            try
            {
                #region App : basic

                this.MessageTemplate_DataInclude_Basic = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region App : search

                this.MessageTemplate_DataInclude_Search = string.Join(",", new List<string>()
                {
                });

                #endregion
                #region App : full

                this.MessageTemplate_DataInclude_Full = string.Join(",", new List<string>()
                {
                    
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
