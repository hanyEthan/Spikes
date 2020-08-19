using System.Threading.Tasks;
using Config.Messaging.Contracts.Messages;
using MassTransit;
using Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Contracts;
using Mcs.Invoicing.Core.Framework.Infrastructure.Logging.Helpers;
using Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Commands.CreateConfigItem;
using Microsoft.Extensions.Logging;

namespace Config.Messaging.Processor.Consumers
{
    public class ConfigMessageConsumer : IAsyncMessage<IConfigCreateCommandMessage>
    {
        #region props.

        private readonly ILogger<ConfigMessageConsumer> _logger;
        private readonly MediatR.IMediator _mediator;

        #endregion
        #region cst.

        public ConfigMessageConsumer(ILogger<ConfigMessageConsumer> logger, 
                                     MediatR.IMediator mediator)
        {
            this._logger = logger;
            this._mediator = mediator;
        }

        #endregion
        #region MassTransit.IConsumer

        public async Task Consume(ConsumeContext<IConfigCreateCommandMessage> context)
        {
            try
            {
                #region LOG
                _logger.LogInformation("{MicroserviceName} : IConfigCreateCommandMessage Received {@MessageId} {@CorrelationId}", LoggingHelpers.ServiceName, context.MessageId, context.CorrelationId);
                _logger.LogDebug("{MicroserviceName} : Started consuming IConfigCreateCommandMessage {@Message}", LoggingHelpers.ServiceName, context.Message);
                #endregion

                var command = Map(context.Message);

                #region LOG
                _logger.LogDebug("{MicroserviceName} : Sending CreateConfigItemCommand {@CreateAuditLogCommand}", LoggingHelpers.ServiceName, command);
                #endregion

                await _mediator.Send(command);
            }
            catch (System.Exception x)
            {
                #region LOG
                _logger.LogError(x, "{MicroserviceName} : Exception occurred when consuming IConfigCreateCommandMessage {@Message} {@MessageId} {@CorrelationId}", LoggingHelpers.ServiceName, context.Message, context.MessageId, context.CorrelationId);
                #endregion
                throw;
            }
        }

        #endregion
        #region helpers.

        private CreateConfigItemCommand Map(IConfigCreateCommandMessage from)
        {
            if (from == null) return null;

            var to = new CreateConfigItemCommand()
            {
                Header = from.Header,
                Description = from.Description,
                Key = from.Key,
                ModuleId = from.ModuleId,
                Value = from.Value,
            };

            return to;
        }

        #endregion
    }
}
