using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Extensions;
using XCore.Framework.Infrastructure.Context.Execution.Handler;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Entities.Repositories.Models;
using XCore.Framework.Infrastructure.Entities.Validation.Contracts;
using XCore.Framework.Infrastructure.Entities.Validation.Models;
using XCore.Services.Docs.Core.Contracts;
using XCore.Services.Docs.Core.DataLayer.Contracts;
using XCore.Services.Docs.Core.Models;
using XCore.Services.Docs.Core.Models.Events.Domain;
using XCore.Services.Docs.Models.Models.Docs;

namespace XCore.Services.Docs.Core.Handlers
{
    public class DocumentHandler : IDocumentHandler
    {
        #region props.

        private readonly IDocumentDataUnity _DataLayer;
        private readonly IModelValidator<Document> DocumentValidator;
        private readonly IMediator _mediator;
        #endregion
        #region cst.
        public DocumentHandler(IDocumentDataUnity dataLayer, IModelValidator<Document> DocumentValidators, IMediator mediator)
        {
            this._DataLayer = dataLayer;
            this.DocumentValidator = DocumentValidators;
            this._mediator = mediator;

            this.Initialized = Initialize();
        }

        #endregion

        #region IUnityService

        public bool? Initialized { get; protected set; }
        public string ServiceId { get { return $"XCore.DocumentHandler.{Guid.NewGuid()}"; } }

        #endregion
        #region IDocumentHandler

        public async Task<ExecutionResponse<SearchResults<Document>>> Get(DocumentSearchCriteria criteria, RequestContext requestContext)
        {
            var context = new ExecutionContext<SearchResults<Document>>();
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

                var results = await this._DataLayer.Document.GetAsync(criteria);
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
        public async Task<ExecutionResponse<Document>> Create(Document document, RequestContext requestContext)
        {
            var context = new ExecutionContext<Models.Document>();
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

                await this._DataLayer.Document.CreateAsync(document);
                await this._DataLayer.SaveAsync();

                #region events.

                await RaiseEvent_DocumentCreated(new List<Document>() { document });

                #endregion

                return context.Response.Set(ResponseState.Success, document);

                #endregion

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                Validation = new ValidationContext<Document>(this.DocumentValidator, document, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<List<Document>>> Create(List<Document> documents, RequestContext requestContext)
        {
            var context = new ExecutionContext<List<Document>>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.

                if (!this.Initialized.GetValueOrDefault())
                {
                    return context.Response.Set(ResponseState.Error, null);
                }

                #endregion
                #region validate.

                var validationResponse = await Validate(documents);
                if (validationResponse != null)
                {
                    return context.Response.Set(validationResponse);
                }

                #endregion
                #region DL

                await this._DataLayer.Document.CreateAsync(documents.ToArray());
                await this._DataLayer.SaveAsync();

                #region events.

                await RaiseEvent_DocumentCreated(documents);

                #endregion

                #endregion

                return context.Response.Set(ResponseState.Success, documents);

                #endregion
            }
            #region context

            , new ActionContext()
            {
                Request = requestContext,
                //Validation = new ValidationContext<Document>(this.DocumentValidator, document, ValidationMode.Create),
            });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<Document>> Edit(Document document, RequestContext requestContext)
        {
            var context = new ExecutionContext<Document>();
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

                var existing = await this._DataLayer.Document.GetFirstAsync(x => x.Id == document.Id);
                MapUpdate(existing, document);
                this._DataLayer.Document.Update(existing);
                await this._DataLayer.SaveAsync();
                return context.Response.Set(ResponseState.Success, document);
                #endregion

                #endregion
            }
            #region context
             , new ActionContext()
             {
                 Request = requestContext,
                 Validation = new ValidationContext<Document>(this.DocumentValidator, document, ValidationMode.Edit),
             });
            return context.Response;

            #endregion
        }
        public async Task<ExecutionResponse<bool>> Delete(int Id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic
                #region validate.

                var Document = await _DataLayer.Document.GetFirstAsync(x => x.Id == Id);
                var validationResponse = await this.DocumentValidator.ValidateAsync(Document, ValidationMode.Delete);
                if (!validationResponse.IsValid)
                {
                    return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                }

                #endregion
                #region DL

                await this._DataLayer.Document.DeleteAsync(Id);
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
        public async Task<ExecutionResponse<bool>> Delete(List<int> Id, RequestContext requestContext)
        {
            var context = new ExecutionContext<bool>();
            await context.Process(async () =>
            {
                #region Logic

                #region validate.
                var Documents = await this._DataLayer.Document.GetAsync(new DocumentSearchCriteria() { Id = Id });
                foreach (var Document in Documents.Results)
                {
                    var validationResponse = await this.DocumentValidator.ValidateAsync(Document, ValidationMode.Delete);
                    if (!validationResponse.IsValid)
                    {
                        return context.Response.Set(ResponseState.ValidationError, false, validationResponse.Errors);
                    }
                }

                #endregion
                #region DL

                this._DataLayer.Document.DeleteList(Documents.Results);
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

        #endregion

        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (this._DataLayer?.Initialized ?? false);
            isValid = isValid && DocumentValidator != null;

            return isValid;
        }

        private bool MapUpdate(Document existing, Document updated)
        {
            if (updated == null || existing == null) return false;

            existing.AttachId = updated.AttachId;
            existing.MetaData = updated.MetaData;
            existing.ModifiedBy = updated.ModifiedBy;
            existing.ModifiedDate = updated.ModifiedDate;

            return true;
        }

        private async Task<ExecutionResponse<bool>> Validate(IList<Document> documents)
        {
            #region model.

            if (documents == null || !documents.Any())
            {
                return new ExecutionResponse<bool>() { State = ResponseState.InvalidInput };
            }

            #endregion
            #region code

            var uniqueCodes = documents.Where(x => !string.IsNullOrWhiteSpace(x.Code))
                               .Select(x => x.Code)
                               .Distinct()
                               .Count();
            if (uniqueCodes != documents.Count)
            {
                return new ExecutionResponse<bool>() { State = ResponseState.ValidationError, DetailedMessages = new List<MetaPair>() { new MetaPair() { Meta = "Code", Property = "Documents : Invalid Code(s)" } } };
            }

            #endregion
            #region AttachmentIds

            var uniqueAttachmentIds = documents.Where(x => !string.IsNullOrWhiteSpace(x.AttachId))
                                               .Select(x => x.AttachId)
                                               .Distinct()
                                               .Count();
            if (uniqueAttachmentIds != documents.Count)
            {
                return new ExecutionResponse<bool>() { State = ResponseState.ValidationError, DetailedMessages = new List<MetaPair>() { new MetaPair() { Meta = "AttachmentId", Property = "Documents : Invalid AttachmentId(s)" } } };
            }

            #endregion
            #region validator.

            foreach (var doc in documents)
            {
                var validationResult = await this.DocumentValidator.ValidateAsync(doc, ValidationMode.Create);
                if (!validationResult.IsValid)
                {
                    return new ExecutionResponse<bool>() { State = ResponseState.ValidationError, DetailedMessages = validationResult.Errors };
                }
            }

            #endregion

            return null;
        }

        #region Event Helper
        private async Task RaiseEvent_DocumentCreated(List<Document> documents)
        {
            if (documents == null || !documents.Any()) return;
            if (this._mediator == null) return;
            if (documents.All(item => item.Id == 0)) return;

            var @event = new DocumentCreatedDomainEvent()
            {
                App = documents.First().App,
                Module = documents.First().Module,
                User = documents.First().UserId,
                Documents = documents.Select(x => new DocumentCreatedMetaData
                {
                    AttachmentId = x.AttachId,
                    DocumentId = x.Id.ToString(),
                }).ToList(),
            };


            await this._mediator.Publish(@event);


        }

        #endregion
        #endregion
    }
}
