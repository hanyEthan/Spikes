using System;
using System.Threading;
using System.Threading.Tasks;
using Mcs.Invoicing.Core.Framework.Infrastructure.Models.Common;
using Mcs.Invoicing.Services.Audit.Client.Sdk.Contracts;
using Mcs.Invoicing.Services.Audit.Client.Sdk.Models;
using Mcs.Invoicing.Services.Config.Application.Common.Contracts;
using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Config.Domain.Events;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Mappers;
using Microsoft.Extensions.Logging;

namespace Mcs.Invoicing.Services.Config.Application.Services.ConfigItems.Commands.CreateConfigItem
{
    public class CreateConfigItemCommandHandler : MediatR.IRequestHandler<CreateConfigItemCommand, BaseResponseContext<int>>
    {
        #region props.

        public bool? Initialized { get; protected set; }

        private readonly ILogger<CreateConfigItemCommandHandler> _logger;
        private readonly IConfigurationDataUnity _dataHandler;
        private readonly IModelMapper<CreateConfigItemCommand, ConfigItem> _modelMapper;
        
        private readonly IConfigEventsPublisher _configEventsPublisher;
        private readonly IAuditService _auditClient;

        #endregion
        #region cst.

        public CreateConfigItemCommandHandler(IConfigurationDataUnity dataHandler,
                                              IConfigEventsPublisher configEventsPublisher,
                                              IAuditService auditClient,
                                              IModelMapper<CreateConfigItemCommand, ConfigItem> modelMapper,
                                              ILogger<CreateConfigItemCommandHandler> logger)
        {
            this._logger = logger;
            this._dataHandler = dataHandler;
            this._modelMapper = modelMapper;

            this._auditClient = auditClient;
            this._configEventsPublisher = configEventsPublisher;

            this.Initialized = Initialize();
        }

        #endregion
        #region MediatR.IRequestHandler

        public async Task<BaseResponseContext<int>> Handle(CreateConfigItemCommand command, CancellationToken cancellationToken)
        {
            try
            {
                #region DL.

                var entity = _modelMapper.Map(command);
                await this._dataHandler.ConfigItems.CreateAsync(entity);
                await this._dataHandler.SaveAsync(cancellationToken);

                #endregion
                #region audit.

                var auditEvent = Map(command, $"created config item.");
                await this._auditClient.CreateAsync(auditEvent);

                #endregion
                #region event.

                var domainEvent = Map(command, entity);
                await this._configEventsPublisher.PublishEvent(domainEvent);

                #endregion

                return command.SetResponse(entity.Id);
            }
            catch (Exception x)
            {
                // todo : log ...
                // note : exceptions should be handled in the layer above this one,
                //        (for example, in case APIs were used, a default exception handling should be coded and configured)
                throw;
            }
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (_dataHandler?.Initialized ?? false);
            isValid = isValid && (_modelMapper != null);
            isValid = isValid && (_configEventsPublisher?.Initialized ?? false);
            isValid = isValid && (_auditClient?.Initialized ?? false);

            return isValid;
        }
        private ConfigCreatedEvent Map(CreateConfigItemCommand command, ConfigItem entity)
        {
            if (command == null || entity == null) return null;
            var @event = new ConfigCreatedEvent()
            {
                Header = command.Header,
                Id = entity.Id,
                Description = entity.Description,
                Key = entity.Key,
                Value = entity.Value,
                ModuleId = entity.ModuleId,
            };

            return @event;
        }
        private AuditMessage Map(CreateConfigItemCommand command, string message)
        {
            if (command == null) return null;

            var to = new AuditMessage()
            {
                Header = command.Header,
                AuditDateTimeUtc = DateTime.UtcNow,
                Description = message,
                ServiceId = Audit.Messaging.Contracts.Enums.AuditsServiceTypes.TaxpayerProfileManagement,  // sample dummy entry, please change it to relevant value.
                EventTypeId = Audit.Messaging.Contracts.Enums.AuditsEventTypes.UserCreated,                // sample dummy entry, please change it to relevant value.
                ObjectTypeId = Audit.Messaging.Contracts.Enums.AuditsObjectTypes.User,                     // sample dummy entry, please change it to relevant value.
                ObjectTypeReferenceId = null,                                                              // sample dummy entry, please change it to relevant value.
                MetaData = null,
                SourceIp = null,
            };

            return to;
        }

        #endregion
    }
}
