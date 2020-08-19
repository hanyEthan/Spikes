using System;
using System.Threading.Tasks;
using MassTransit;
using XCore.Framework.Infrastructure.Context.Services.Handlers;
using XCore.Framework.Infrastructure.Context.Services.Models;
using XCore.Services.Audit.API.Mappers;
using XCore.Services.Audit.API.Models;
using XCore.Services.Audit.Core.Contracts;
using XCore.Services.Audit.Core.Models;
using XCore.Services.Audit.Models.Contracts;

namespace XCore.Services.Audit.API.Events.Consumers
{
    public class AuditMessageConsumer : IConsumer<IAuditMessage>
    {
        #region props.

        private readonly IAuditHandler auditHandler;

        #endregion
        #region cst.

        public AuditMessageConsumer(IAuditHandler auditHandler)
        {
            //this.auditHandler = (IAuditHandler) serviceProvider.GetService(typeof(IAuditHandler));
            this.auditHandler = auditHandler;
        }

        #endregion
        #region MassTransit.IConsumer

        public async Task Consume(ConsumeContext<IAuditMessage> context)
        {
            #region request.

            bool isValidRequest;
            ServiceExecutionRequestDTO<AuditTrailDTO> requestDTO = Map(context);
            ServiceExecutionRequest<AuditTrail> requestDMN;
            ServiceExecutionResponse<AuditTrail> responseDMN;
            ServiceExecutionResponseDTO<bool> responseDTO;

            // ...
            responseDTO = ServiceExecutionContext.HandleRequestDTO<AuditTrailDTO, AuditTrail, bool, AuditTrail>(requestDTO, AuditMapper.Instance, out requestDMN, out isValidRequest, httpRequest: null);

            #endregion
            #region Bl.
            var domainResponse = this.auditHandler.Create(requestDMN.Content, requestDMN.ToRequestContext());
            //if(domainResponse.State.)
            if (domainResponse.Result!=null)
            {
                await complete();
            }
            #endregion
        }

        #endregion
        #region helpers.
        private ServiceExecutionRequestDTO<AuditTrailDTO> Map(ConsumeContext<IAuditMessage> context)
        {
           
                var to = new ServiceExecutionRequestDTO<AuditTrailDTO>();
                var auditTrailDTO = new AuditTrailDTO();
                auditTrailDTO.App = context.Message.App;
                auditTrailDTO.ConnectionMethod = context.Message.ConnectionMethod;
                auditTrailDTO.CreatedBy = context.Message.CreatedBy;
                context.Message.DestinationAddress =auditTrailDTO.DestinationAddress;
                context.Message.DestinationIP =auditTrailDTO.DestinationIP;
                context.Message.DestinationPort =auditTrailDTO.DestinationPort;
                context.Message.Entity =auditTrailDTO.Entity;
                context.Message.MetaData =auditTrailDTO.MetaData;
                context.Message.ModifiedBy =auditTrailDTO.ModifiedBy;
                context.Message.Module =auditTrailDTO.Module;
                context.Message.SourceClient =auditTrailDTO.SourceClient;
                context.Message.SourceIP =auditTrailDTO.SourceIP;
                context.Message.SourceOS =auditTrailDTO.SourceOS;
                context.Message.SourcePort =auditTrailDTO.SourcePort;
                context.Message.Text =auditTrailDTO.Text;
                context.Message.UserId =auditTrailDTO.UserId;
                context.Message.UserName =auditTrailDTO.UserName;
                auditTrailDTO.SyncStatus = context.Message.SyncStatus;
                auditTrailDTO.Level = context.Message.Level;
                auditTrailDTO.Action = context.Message.Action;
                to.Content = auditTrailDTO;

                return to;
            
            
        }
        public Task ThrowNullException()
        {
            return Task.Run(() => {

                throw new NullReferenceException();
            });
        }
        public Task ThrowNotImplementedException()
        {
            return Task.Run(() => {

                throw new NotImplementedException();
            });
        }
        public Task complete()
        {
            return Task.Run(() => {
 
               
            });
        }

        #endregion
    }
}
