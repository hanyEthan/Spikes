using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Extensions;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Context.Services.Mappers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Framework.Utilities;
using XCore.Services.Docs.SDK.Contracts;
using XCore.Services.Docs.SDK.Models;
using XCore.Services.Notifications.Core.Contracts;
using XCore.Services.Notifications.Core.Models.Domain;
using XCore.Services.Notifications.Core.Models.Events.Domain;
using XCore.Services.Notifications.Core.Models.Support;
using XCore.Services.Notifications.Models.Models.Notifications.MessageTemplate;

namespace XCore.Services.Notifications.Core.Handlers
{
    public class MessageTemplatesHandler : IMessageTemplatesHandler
    {
        #region props.

        private string MessageTemplate_DataInclude_Basic { get; set; }
        private string MessageTemplate_DataInclude_Search { get; set; }
        private string MessageTemplate_DataInclude_Full { get; set; }

        private readonly IDocumentClient DocumentsService;
        private readonly INotificationsDataUnity _DataHandler;
        private readonly IModelValidator<MessageTemplate> MessageTemplateValidator;

        private readonly IModelValidator<ResolveRequest> ResolveRequestValidator;
        private readonly IMediator _mediator;
        #endregion
        #region cst.

        public MessageTemplatesHandler(INotificationsDataUnity dataHandler, IModelValidator<MessageTemplate> MessageTemplateValidators, IModelValidator<ResolveRequest> ResolveRequestValidator, IDocumentClient DocumentClient, IMediator _mediator)
        {
            this.MessageTemplateValidator = MessageTemplateValidators;
            this.ResolveRequestValidator = ResolveRequestValidator;
            this._DataHandler = dataHandler;
            this.DocumentsService = DocumentClient;
            this._mediator = _mediator;

            this.Initialized = this.Initialize();
        }

        #endregion
        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.MessageTemplatesHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region Actions

        public async Task<ExecutionResponse<SearchResults<MessageTemplate>>> Get(MessageTemplateSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<MessageTemplate>>();
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

                var apps = await _DataHandler.MessageTemplate.GetAsync(criteria, includes);
                return context.Response.Set(ResponseState.Success, apps);

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
        public async Task<ExecutionResponse<MessageTemplate>> Create(MessageTemplate messageTemplate, RequestContext requestContext)
        {
            var context = new ExecutionContext<MessageTemplate>();
            await context.Process(async () =>
            {
                #region Logic

                #region Integration : documents service

                var integrationResponse = await CreateDocumentsInRemoteService(messageTemplate.Attachments, requestContext);
                if(integrationResponse.State != ResponseState.Success) return context.Response.Set(integrationResponse);

                #endregion
                #region DL.

                await _DataHandler.MessageTemplate.CreateAsync(messageTemplate);
                await _DataHandler.SaveAsync();

                #endregion

                return context.Response.Set(ResponseState.Success, messageTemplate);

                #endregion
            }
            #region context

             , new ActionContext()
             {
                 Request = requestContext,
                 Validation = new ValidationContext<MessageTemplate>(this.MessageTemplateValidator, messageTemplate, ValidationMode.Create),
             });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<MessageTemplate>> Edit(MessageTemplate messageTemplate, RequestContext requestContext)
        {
            var context = new ExecutionContext<MessageTemplate>();
            await context.Process(async () =>
            {
                #region Logic

                #region update

                var existing = await _DataHandler.MessageTemplate.GetFirstAsync(x => x.Id == messageTemplate.Id || x.Code == messageTemplate.Code, null, this.MessageTemplate_DataInclude_Full);
                if (existing == null)
                {
                    return context.Response.Set(ResponseState.NotFound, messageTemplate);
                }
                MapUpdate(existing, messageTemplate, out List<MessageTemplateAttachment> addedAttachments, out List<MessageTemplateAttachment> removedAttachments);

                #endregion
                #region Integration : documents service
                if (addedAttachments.Count > 0)
                {
                    var integrationResponse = await CreateDocumentsInRemoteService(addedAttachments, requestContext);
                    if (integrationResponse.State != ResponseState.Success) return context.Response.Set(integrationResponse);
                }

                #endregion
                #region DL

                _DataHandler.MessageTemplate.Update(existing);
                await _DataHandler.SaveAsync();

                #endregion
                #region Events.
                if (removedAttachments.Count > 0)
                {
                    await RaiseEvent_DeleteAttachments(messageTemplate, removedAttachments, requestContext);
                }
                #endregion

                return context.Response.Set(ResponseState.Success, existing);

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<MessageTemplate>(this.MessageTemplateValidator, messageTemplate, ValidationMode.Edit),
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

                var existing = await _DataHandler.MessageTemplate.GetFirstAsync(x => x.Id == id,null,this.MessageTemplate_DataInclude_Full);
                if (existing == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }

                #region validation.

                var validationResponse = await this.MessageTemplateValidator.ValidateAsync(existing, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                _DataHandler.MessageTemplate.Delete(existing);
                await _DataHandler.SaveAsync();

                #endregion
                #region Events.

                await RaiseEvent_DeleteAttachments(existing, existing.Attachments, requestContext);

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

                var existing = await _DataHandler.MessageTemplate.GetFirstAsync(x => x.Code == code);
                if (existing == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }

                #region validation.

                var validationResponse = await this.MessageTemplateValidator.ValidateAsync(existing, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                _DataHandler.MessageTemplate.Delete(existing);
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
        public async Task<ExecutionResponse<bool>> Activate(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await _DataHandler.MessageTemplate.GetFirstAsync(x => x.Id == id);
                if (existing == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }

                #region validation.
                var validationResponse = await this.MessageTemplateValidator.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await _DataHandler.MessageTemplate.SetActivationAsync(id, true);
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
        public async Task<ExecutionResponse<bool>> Activate(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await _DataHandler.MessageTemplate.GetFirstAsync(x => x.Code == code);
                if (existing == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }

                #region validation.
                var validationResponse = await this.MessageTemplateValidator.ValidateAsync(existing, ValidationMode.Activate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await _DataHandler.MessageTemplate.SetActivationAsync(existing.Id, true);
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
        public async Task<ExecutionResponse<bool>> Deactivate(int id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await _DataHandler.MessageTemplate.GetFirstAsync(x => x.Id == id);
                if (existing == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }

                #region validation.
                var validationResponse = await this.MessageTemplateValidator.ValidateAsync(existing, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await _DataHandler.MessageTemplate.SetActivationAsync(id, false);
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
        public async Task<ExecutionResponse<bool>> Deactivate(string code, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var existing = await _DataHandler.MessageTemplate.GetFirstAsync(x => x.Code == code);
                if (existing == null)
                {
                    return context.Response.Set(ResponseState.NotFound, false);
                }
                #region validation.
                var validationResponse = await this.MessageTemplateValidator.ValidateAsync(existing, ValidationMode.Deactivate);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion

                await _DataHandler.MessageTemplate.SetActivationAsync(existing.Id, false);
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
        public async Task<ExecutionResponse<bool>> IsUnique(MessageTemplate app, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await _DataHandler.MessageTemplate.AnyAsync(x => ((app.Name != null && x.Name == app.Name.Trim()) ||
                                                                                   (app.NameCultured != null && x.NameCultured == app.NameCultured.Trim()))
                                                                                   &&
                                                                                   (x.Code != app.Code)
                                                                                   &&
                                                                                   (x.IsActive == true));

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
        public async Task<ExecutionResponse<bool>> IsExists(MessageTemplateSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region DL

                var isExisting = await _DataHandler.MessageTemplate.AnyAsync(criteria);
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
        public async Task<ExecutionResponse<ResolveResponse>> Resolve(ResolveRequest request, RequestContext requestContext)
        {
            var context = new ExecutionContext<ResolveResponse>();
            await context.Process(async () =>
            {
                #region Logic

                // get template ...
                var messageTemplate = await _DataHandler.MessageTemplate.GetFirstAsync(x => x.Id == request.MessageTemplateId, null, this.MessageTemplate_DataInclude_Full);
                var result = messageTemplate.Body;

                // build response ...
                var response = new ResolveResponse()
                {
                    RequestId = request.RequestId,
                    MessageTemplateId = request.MessageTemplateId,
                    Result = messageTemplate.Body,
                };

                // resolve ...
                foreach (var valueItem in request.Values)
                {
                    response.Result = response.Result.Replace(valueItem.Key, valueItem.Value);
                }

                // return ...
                return context.Response.Set(ResponseState.Success, response);

                #endregion
            }
            #region context

             , new ActionContext()
             {
                 Request = requestContext,
                 Validation = new ValidationContext<ResolveRequest>(this.ResolveRequestValidator, request, ValidationMode.Create),

             });
            return context.Response;

            #endregion
        }

        #endregion
        #region helpers.

        #region Map Updates.

        private bool MapUpdate(MessageTemplate existing, MessageTemplate updated, out List<MessageTemplateAttachment> addedAttachments, out List<MessageTemplateAttachment> removedAttachments)
        {
            addedAttachments = new List<MessageTemplateAttachment>();
            removedAttachments = new List<MessageTemplateAttachment>();

            if (updated == null || existing == null) return false;

            existing.Code = updated.Code;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;
            existing.Name = updated.Name;
            existing.NameCultured = updated.NameCultured;
            existing.AppId = updated.AppId;
            existing.ModuleId = updated.ModuleId;
            existing.Body = updated.Body;
            existing.Title = updated.Title;

            bool isValid = true;

            //isValid = isValid && MapUpdate(existing.Keys, updated.Keys);
            isValid = isValid && ExtractUpdate(existing.Attachments, updated.Attachments, out addedAttachments, out removedAttachments);

            existing.Keys = updated.Keys;
            existing.Attachments = updated.Attachments;

            return isValid;
        }

        #region obsolete.

        //private bool MapUpdate(IList<MessageTemplateKey> existing, IList<MessageTemplateKey> updated)
        //{
        //    // ...
        //    if (updated == null || existing == null) return false;

        //    // get [deleted records] ...
        //    foreach (var existingItem in existing)
        //    {
        //        var isDeleted = !updated.Any(x => x.Id == existingItem.Id);

        //        // delete the [existingItem]
        //        if (isDeleted)
        //        {
        //            existing.Remove(existingItem);
        //        }
        //    }

        //    // get [inserted records] ...
        //    foreach (var updatedItem in updated)
        //    {
        //        var isInserted = !existing.Any(x => x.Id == updatedItem.Id);

        //        // insert the [updatedItem]
        //        if (isInserted)
        //        {
        //            existing.Add(updatedItem);
        //        }
        //    }

        //    // Map
        //    foreach (var existingItem in existing)
        //    {
        //        var updatedItem = updated.FirstOrDefault(x => x.Id == existingItem.Id && x.Id != 0);
        //        if (updatedItem == null) continue;

        //        MapUpdate(existingItem, updatedItem);
        //    }

        //    return true;
        //}
        //private bool MapUpdate(MessageTemplateKey existing, MessageTemplateKey updated)
        //{
        //    if (updated == null || existing == null) return false;

        //    existing.Code = updated.Code;
        //    existing.MetaData = updated.MetaData;
        //    existing.ModifiedBy = updated.ModifiedBy;
        //    existing.ModifiedDate = updated.ModifiedDate;
        //    existing.Key = updated.Key;
        //    existing.Description = updated.Description;

        //    return true;
        //}

        #endregion

        private bool ExtractUpdate(List<MessageTemplateAttachment> existing, List<MessageTemplateAttachment> updated, 
                                   out List<MessageTemplateAttachment> addedAttachments, out List<MessageTemplateAttachment> removedAttachments)
        {
            existing = existing ?? new List<MessageTemplateAttachment>();
            updated = updated ?? new List<MessageTemplateAttachment>();

            addedAttachments = updated.Except(updated.Where(x => existing.Any(y => y.AttachmentReferenceId == x.AttachmentReferenceId)).ToList()).ToList();
            removedAttachments = existing.Except(existing.Where(x => updated.Any(y => y.AttachmentReferenceId == x.AttachmentReferenceId)).ToList()).ToList();

            return true;
        }

        #endregion
        #region Integration : documents service

        private async Task<ExecutionResponse<MessageTemplate>> CreateDocumentsInRemoteService(List<MessageTemplateAttachment> attachments, RequestContext requestContext)
        {
            #region validate.

            if (attachments == null || !attachments.Any()) return new ExecutionResponse<MessageTemplate>() { State = ResponseState.Success };

            #endregion
            #region call.

            var request = MapCreateRequest(attachments, requestContext);
            var response = await this.DocumentsService.Create(request);

            #endregion
            #region response.

            #region error ?

            if (response?.Response?.ResponseCode != (int?)ResponseState.Success)
            {
                return new ExecutionResponse<MessageTemplate>()
                {
                    State = (ResponseState?)response?.Response?.ResponseCode ?? ResponseState.Error,
                    DetailedMessages = string.IsNullOrWhiteSpace(response?.Response?.ResponseMessage) ? new List<MetaPair>() : new List<MetaPair>()
                    {
                        new MetaPair(){ Property = "Attachments" , Meta = response.Response.ResponseMessage },
                    }
                };
            }

            #endregion

            bool state = MapResponse(attachments, response.Response);
            if (!state) return new ExecutionResponse<MessageTemplate>() { State = ResponseState.Error };

            #endregion

            return new ExecutionResponse<MessageTemplate>() { State = ResponseState.Success, };
        }
        private ServiceExecutionRequestDTO<List<DocumentDTO>> MapCreateRequest(List<MessageTemplateAttachment> from, RequestContext requestContext)
        {
            if (from == null || requestContext == null) return null;

            var to = ServiceExecutionMappers.Map<List<DocumentDTO>>(requestContext);
            if (to == null) return null;

            to.Content = new List<DocumentDTO>();

            foreach (var fromItem in from)
            {
                var toItem = MapRequest(fromItem);
                if (toItem == null) return null;
                else to.Content.Add(toItem);
            }

            return to;
        }
        private DocumentDTO MapRequest(MessageTemplateAttachment from)
        {
            if (from == null) return null;

            var to = new DocumentDTO();

            to.AttachId = from.AttachmentReferenceId;
            to.Action = from.Action;
            to.App = from.App;
            to.Category = from.Category;
            to.Code = from.Code;
            to.CreatedBy = from.CreatedBy;
            to.Entity = from.MessageTemplateId.ToString();
            to.MetaData = from.MetaData;
            to.Module = from.Module;
            to.UserId = from.UserId;
            to.UserName = from.UserName;

            return to;
        }
        private bool MapResponse(List<MessageTemplateAttachment> attachments, ServiceExecutionResponseDTO<List<DocumentDTO>> documentDTOs)
        {
            if (attachments == null || documentDTOs?.Content == null) return false;

            foreach (var templateAttachment in attachments)
            {
                var docDTO = documentDTOs.Content.Where(x => x.AttachId == templateAttachment.AttachmentReferenceId && !string.IsNullOrWhiteSpace(templateAttachment.AttachmentReferenceId)).FirstOrDefault();
                if (docDTO == null || docDTO.Id == 0) return false;

                templateAttachment.DocumentReferenceId = docDTO.Id.ToString();
            }

            return true;
        }

        #endregion
        #region events.

        private async Task RaiseEvent_DeleteAttachments(MessageTemplate messageTemplate, List<MessageTemplateAttachment> documents, RequestContext requestContext)
        {
            if (documents == null || !documents.Any()) return;
            if (messageTemplate == null) return;
            if (requestContext == null) return;
            if (this._mediator == null) return;

            #region map.

            var @event = new MessageTemplateAttachmentsDeletedDomainEvent()
            {
                App = messageTemplate.AppId,
                Module = messageTemplate.ModuleId,
                User = requestContext.UserId,
                Attachments = documents.Select(x => new NotificationMessageTemplateAttachmentMetadata
                {
                    AttachmentReferenceId = x.AttachmentReferenceId,
                    DocumentReferenceId = x.DocumentReferenceId,
                    NotificationMessageTemplateId = x.MessageTemplateId.ToString(),
                }).ToList(),
            };

            #endregion

            await this._mediator.Publish(@event);
        }

        #endregion

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && InitializeIncludes();

            isValid = isValid && (this._DataHandler?.Initialized ?? false);
            isValid = isValid && MessageTemplateValidator != null;

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
                    "Keys",
                    "Attachments",
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
