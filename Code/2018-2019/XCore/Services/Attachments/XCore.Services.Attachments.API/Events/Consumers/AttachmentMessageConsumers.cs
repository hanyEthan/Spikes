using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XCore.Framework.Infrastructure.Context.Execution.Models;
using XCore.Framework.Infrastructure.Context.Execution.Support;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Attachments.Core.Contracts;
using XCore.Services.Attachments.Core.Models;
using XCore.Services.Attachments.Core.Models.Enums;
using XCore.Services.Attachments.Core.Models.Support;
using XCore.Services.Docs.Models.Contracts.Docs;
using XCore.Services.Docs.SDK.Contracts;
using XCore.Services.Docs.SDK.Models;
using XCore.Services.Notifications.Models.Contracts.Notifications;

namespace XCore.Services.Attachments.API.Events.Consumers
{
    public class AttachmentMessageConsumers : IConsumer<IDocumentCreatedIntegrationEvent>,
                                              IConsumer<INotificationMessageTemplateAttachmentsDeletedIntegrationEvent>
    { 
        #region props.

        private readonly IAttachmentsHandler AttachmentHandler;
        private readonly IDocumentClient DocumentService;
        public bool? Initialized { get; protected set; }

        #endregion
        #region cst.

        public AttachmentMessageConsumers(IAttachmentsHandler attachmentHandler, IDocumentClient DocumentService)
        {
            this.AttachmentHandler = attachmentHandler;
            this.DocumentService = DocumentService;
            this.Initialized = Initialize();
        }

        #endregion

        #region MassTransit.IConsumer<IDocumentCreatedIntegrationEvent>

        public async Task Consume(ConsumeContext<IDocumentCreatedIntegrationEvent> context)
        {
            Check();

            var request = Map(context.Message);

            var Attachments = await this.AttachmentHandler.Get(request, SystemRequestContext.Instance);
            var response = await this.AttachmentHandler.CreateConfirm(Attachments.Result.Results, SystemRequestContext.Instance);
            if (response.State != ResponseState.Success)
            {
                throw new Exception($"Event handling for Document {context.Message.Documents}");
            }
        }

        #endregion
        #region IConsumer<INotificationMessageTemplateAttachmentsDeletedIntegrationEvent>

        public async Task Consume(ConsumeContext<INotificationMessageTemplateAttachmentsDeletedIntegrationEvent> context)
        {
            #region Document service.

            #endregion
            #region Bl.
            var AttachmentIds = context.Message.Attachments.Select(x => x.AttachmentReferenceId).ToList();

            var domainResponse = await this.AttachmentHandler.DeleteListConfirm(AttachmentIds, new Framework.Infrastructure.Context.Execution.Support.RequestContext());
            if (domainResponse.State != ResponseState.Success)
            {
                throw new Exception($"Event handling for Attachment {context.Message.Attachments}");
            }
            #endregion
        }

        #endregion

        #region helpers.

        private AttachmentSearchCriteria Map(IDocumentCreatedIntegrationEvent from)
        {
            if (from?.Documents == null) return null;
            if (!from.Documents.Any()) return null;

            //return new AttachmentConfirmationAction()
            //{
            //    AttachmentIds = from.Documents.Select(X => X.AttachmentId).ToList(),
            //    ConfirmationAction = AttachmentConfirmationStatus.ConfirmAdd,
            //};
            //List<Attachment> attachments = new List<Attachment>() ;
            //attachments = from.Documents.Select(x => new Attachment() { Id = x.AttachmentId }).ToList();
            //return attachments;
            List<string> Ids = from.Documents.Select(x =>  x.AttachmentId ).ToList();
            return new AttachmentSearchCriteria() {Id= Ids };
        }

        private void Check()
        {
            if (this.Initialized == false) throw new Exception("not initialized correctly.");
        }
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (this.AttachmentHandler?.Initialized ?? false);
            isValid = isValid && (this.DocumentService?.Initialized ?? false);

            return isValid;
        }

        #endregion
    }
}
