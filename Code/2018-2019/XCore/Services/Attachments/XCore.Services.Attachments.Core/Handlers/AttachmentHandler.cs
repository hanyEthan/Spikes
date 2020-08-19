using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Config.Contracts;
using XCore.Framework.Infrastructure.Context.Execution.Extensions;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Attachments.Core.Contracts;
using XCore.Services.Attachments.Core.DataLayer.Contracts;
using XCore.Services.Attachments.Core.Models;
using XCore.Services.Attachments.Core.Models.Enums;
using XCore.Services.Attachments.Core.Models.Support;
using XCore.Services.Attachments.Core.Support.Config;

namespace XCore.Services.Attachments.Core.Handlers
{
    public class AttachmentHandler : IAttachmentsHandler
    {
        #region props.

        private readonly IAttachmentDataUnity _DataLayer;
        private readonly IModelValidator<Attachment> AttachmentValidator;
        private readonly IConfigProvider<AttachmentServiceConfig> ConfigrationProvider;
        private AttachmentServiceConfig Configration;

        #endregion
        #region cst.

        public AttachmentHandler(IAttachmentDataUnity dataLayer,
                                 IConfigProvider<AttachmentServiceConfig> configrationProvider,
                                 IModelValidator<Attachment> AttachmentValidator)
        {
            this._DataLayer = dataLayer;
            this.AttachmentValidator = AttachmentValidator;
            this.ConfigrationProvider = configrationProvider;
            this.Initialized = Initialize();
        }

        #endregion

        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.AttachmentsHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region IAttachmentsHandler
        public async Task<ExecutionResponse<SearchResults<Attachment>>> Get(AttachmentSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<Attachment>>();
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

                var results = await this._DataLayer.Attachments.GetAsync(criteria);
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
        public async Task<ExecutionResponse<Attachment>> Create(Attachment attachment, RequestContext requestContext)
        {
            var context = new ExecutionContext<Attachment>();
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

                attachment.Status =
               (Configration.ConfirmationMode == false) ? AttachmentStatus.Permanent : AttachmentStatus.MarkedForAddtion;

                await this._DataLayer.Attachments.CreateAsync(attachment);
                await this._DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, attachment);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Attachment>(this.AttachmentValidator, attachment, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<List<Attachment>>> CreateConfirm(List<Attachment> attachments, RequestContext requestContext)
        {
            var context = new ExecutionContext<List<Attachment>>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                if (Configration.ConfirmationMode == false)
                {
                    return context.Response.Set(ResponseState.Success, attachments);
                }

                if (!this.Initialized.GetValueOrDefault())
                {
                    return context.Response.Set(ResponseState.Error, null);
                }

                var existing = await this._DataLayer.Attachments.GetAsync(new AttachmentSearchCriteria() { Id = attachments.Select(x => x.Id).ToList() });

                if (existing == null || existing.Results.Count != attachments.Count)
                {
                    return context.Response.Set(ResponseState.NotFound, null);

                }
                #endregion
                #region DL

                this._DataLayer.Attachments.CreateConfirm(attachments);
                await this._DataLayer.SaveAsync();
                return context.Response.Set(ResponseState.Success, attachments);

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
        public async Task<ExecutionResponse<bool>> DeleteSoft(string id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.
                var Attachment = await this._DataLayer.Attachments.GetAsync(new AttachmentSearchCriteria() { Id = new List<string> { id } });

                if (Attachment == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }
                #endregion
                #region DL
                if (Configration.ConfirmationMode == false)
                {
                    await Delete(id, requestContext);
                }
                else
                {
                    this._DataLayer.Attachments.DeleteSoft(Attachment.Results[0]);
                    await this._DataLayer.SaveAsync();
                }
                return context.Response.Set(ResponseState.Success, true);


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
        public async Task<ExecutionResponse<bool>> DeleteListConfirm(List<string> id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.
                if (Configration.ConfirmationMode == false)
                {
                    return context.Response.Set(ResponseState.Success, true);
                }
                var Attachments = await this._DataLayer.Attachments.GetAsync(new AttachmentSearchCriteria() { Id = id });
                foreach (var Attachment in Attachments.Results)
                {
                    var validationResponse = await this.AttachmentValidator.ValidateAsync(Attachment, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }
                }

                #endregion
                #region DL

                this._DataLayer.Attachments.DeleteList(Attachments.Results);
                await this._DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, true);


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
        public async Task<ExecutionResponse<bool>> Delete(string id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                //var Attachment = await this._DataLayer.Attachments.GetFirstAsync(x => x.Id == id);
                var Attachment = new Attachment() { Id = id };
                var validationResponse = await this.AttachmentValidator.ValidateAsync(Attachment, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await this._DataLayer.Attachments.DeleteAsync(id);
                await this._DataLayer.SaveAsync();

                return context.Response.Set(ResponseState.Success, true);


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
        public async Task<ExecutionResponse<bool>> IsExists(AttachmentSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await _DataLayer.Attachments.AnyAsync(criteria);
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
        public async Task<ExecutionResponse<bool>> IsUnique(Attachment attachment, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await _DataLayer.Attachments.AnyAsync(x => (x.Name == attachment.Name.Trim())
                                                                       &&
                                                                       (x.Code != attachment.Code));

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

            isValid = isValid && (this._DataLayer?.Initialized ?? false);
            isValid = isValid && AttachmentValidator != null;
            isValid = isValid && (this.ConfigrationProvider?.Initialized ?? false);
            isValid = isValid && InitializeConfig().GetAwaiter().GetResult();

            return isValid;
        }
        private async Task<bool> InitializeConfig()
        {
            this.Configration = await this.ConfigrationProvider.GetConfigAsync();
            return this.Configration != null;
        }

        #endregion
    }
}
