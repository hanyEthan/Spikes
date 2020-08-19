using MassTransit;
using System;
using System.Linq;
using System.Threading.Tasks;
using XCore.Services.Docs.Core.Contracts;
using XCore.Services.Notifications.Models.Contracts.Notifications;

namespace XCore.Services.Docs.API.Events.Consumers
{
    public class DocumentMessageConsumers : IConsumer<INotificationMessageTemplateAttachmentsDeletedIntegrationEvent>
    { 
        #region props.

        private readonly IDocumentHandler DocumentHandler;
        public bool? Initialized { get; protected set; }



        #endregion
        #region cst.

        public DocumentMessageConsumers(IDocumentHandler DocumentHandler)
        {
            this.DocumentHandler = DocumentHandler;
            this.Initialized = Initialize();
        }

        #endregion
        #region MassTransit.IConsumer
       
        public async Task Consume(ConsumeContext<INotificationMessageTemplateAttachmentsDeletedIntegrationEvent> context)
        {
            #region Document service.

            #endregion
            #region Bl.
            var DocumentIds = context.Message.Attachments.Select(x => Int32.Parse(x.DocumentReferenceId)).ToList();

            var domainResponse = await this.DocumentHandler.Delete(DocumentIds, new Framework.Infrastructure.Context.Execution.Support.RequestContext());
            if (domainResponse.State != Framework.Infrastructure.Context.Execution.Models.ResponseState.Success)
            {
                throw new Exception($"Event handling for Documents {context.Message.Attachments}");
            }
            #endregion
        }

        #endregion
        #region helpers.
       
        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (this.DocumentHandler?.Initialized ?? false);

            return isValid;
        }

        #endregion
    }
}
