using System;
using System.Threading;
using System.Threading.Tasks;
using XCore.Framework.Framework.ServiceBus.Contracts;
using XCore.Services.IntegrationModels.Security;
using XCore.Services.Security.Core.Models.Events.Domain;
using XCore.Services.Security.Core.Models.Events.Integration;

namespace XCore.Services.Security.API.Events.Publishers
{
    public class EventsPublisher : MediatR.INotificationHandler<RolesAssociatedToActorDomainEvent>, 
                                   MediatR.INotificationHandler<RolesDeassociatedFromActorDomainEvent>,
                                   MediatR.INotificationHandler<PrivilegeAssociatedToActorDomainEvent>,
                                   MediatR.INotificationHandler<PrivilegeDeassociatedFromActorDomainEvent>,
                                   MediatR.INotificationHandler<RoleCreatedDomainEvent>, 
                                   MediatR.INotificationHandler<RoleDeletedDomainEvent>,
                                   MediatR.INotificationHandler<RoleActivatedDomainEvent>, 
                                   MediatR.INotificationHandler<RoleDeactivatedDomainEvent>,
                                   MediatR.INotificationHandler<PrivilegeAssociatedToRoleDomainEvent>, 
                                   MediatR.INotificationHandler<PrivilegeDeassociatedFromRoleDomainEvent>,
                                   MediatR.INotificationHandler<ClaimsAssociatedToActorDomainEvent>,
                                   MediatR.INotificationHandler<ClaimsDeassociatedFromActorDomainEvent>,
                                   MediatR.INotificationHandler<ClaimsAssociatedToroleDomainEvent>,
                                   MediatR.INotificationHandler<ClaimsDeassociatedFromRoleDomainEvent>
    {
        #region props.

        public bool? Initialized { get; protected set; }
        protected IServiceBus SB { get; set; }

        #endregion
        #region cst.

        public EventsPublisher(IServiceBus sB)
        {
            this.SB = sB;
            this.Initialized = this.Initialize();
        }

        #endregion

        #region ISecurityEventsPublisher.
        public async Task Handle(RoleDeletedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();

                await SB.Publish<IRoleDeletedIntegrationEvent, RoleDeletedIntegrationEvent>(new RoleDeletedIntegrationEvent()
                {
                    App = notification.App,
                    Code = notification.Code,
                    Action = notification.Action,
                    EventCorrelationId = notification.EventCorrelationId,
                    EventCreatedDateTime = notification.EventCreatedDateTime,
                    EventId = notification.EventId,
                    EventResponse = notification.EventResponse,
                    Model = notification.Model,
                    Module = notification.Module,
                    User = notification.User
                });
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }

        }
        public async Task Handle(RoleCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();

                await SB.Publish<IRoleCreatedIntegrationEvent, RoleCreatedIntegrationEvent>(new RoleCreatedIntegrationEvent()
                {
                    App = notification.App,
                    RoleId = notification.RoleId,
                    Action = notification.Action,
                    EventCorrelationId = notification.EventCorrelationId,
                    EventCreatedDateTime = notification.EventCreatedDateTime,
                    EventId = notification.EventId,
                    EventResponse = notification.EventResponse,
                    Model = notification.Model,
                    Module = notification.Module,
                    User = notification.User
                });
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }
        public async Task Handle(PrivilegeDeassociatedFromActorDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();

                await SB.Publish<IPrivilegeDeassociatedFromActorIntegrationEvent, PrivilegeDeassociatedFromActorIntegrationEvent>(new PrivilegeDeassociatedFromActorIntegrationEvent()
                {
                    ActorId = notification.ActorId,
                    Privileges = notification.Privileges,
                    Action = notification.Action,
                    EventCorrelationId = notification.EventCorrelationId,
                    EventCreatedDateTime = notification.EventCreatedDateTime,
                    EventId = notification.EventId,
                    EventResponse = notification.EventResponse,
                    Model = notification.Model,
                    Module = notification.Module,
                    User = notification.User
                });
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }
        public async Task Handle(PrivilegeAssociatedToActorDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();

                await SB.Publish<IPrivilegeAssociatedToActorIntegrationEvent, PrivilegeAssociatedToActorIntegrationEvent>(new PrivilegeAssociatedToActorIntegrationEvent()
                {
                    App = notification.App,
                    ActorId = notification.ActorId,
                    Privileges = notification.Privileges,
                    Action = notification.Action,
                    EventCorrelationId = notification.EventCorrelationId,
                    EventCreatedDateTime = notification.EventCreatedDateTime,
                    EventId = notification.EventId,
                    EventResponse = notification.EventResponse,
                    Model = notification.Model,
                    Module = notification.Module,
                    User = notification.User
                });
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }
        public async Task Handle(RolesDeassociatedFromActorDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();

                await SB.Publish<IRoleDeassociatedFromActorIntegrationEvent, RoleDeassociatedFromActorIntegrationEvent>(new RoleDeassociatedFromActorIntegrationEvent()
                {
                    ActorId = notification.ActorId,
                    Roles = notification.Roles,
                    Action = notification.Action,
                    EventCorrelationId = notification.EventCorrelationId,
                    EventCreatedDateTime = notification.EventCreatedDateTime,
                    EventId = notification.EventId,
                    EventResponse = notification.EventResponse,
                    Model = notification.Model,
                    Module = notification.Module,
                    User = notification.User
                });
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }
        public async Task Handle(RolesAssociatedToActorDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();

                await SB.Publish<IRoleAssociatedToActorIntegrationEvent, RoleAssociatedToActorIntegrationEvent>(new RoleAssociatedToActorIntegrationEvent()
                {
                    App = notification.App,
                    ActorId = notification.ActorId,
                    Roles = notification.Roles,
                    Action = notification.Action,
                    EventCorrelationId = notification.EventCorrelationId,
                    EventCreatedDateTime = notification.EventCreatedDateTime,
                    EventId = notification.EventId,
                    EventResponse = notification.EventResponse,
                    Model = notification.Model,
                    Module = notification.Module,
                    User = notification.User
                });
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }

        public async Task Handle(RoleActivatedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();

                await SB.Publish<IRoleActivatedIntegrationEvent, RoleActivatedIntegrationEvent>(new RoleActivatedIntegrationEvent()
                {
                    App = notification.App,
                    Code = notification.Code,
                    Action = notification.Action,
                    EventCorrelationId = notification.EventCorrelationId,
                    EventCreatedDateTime = notification.EventCreatedDateTime,
                    EventId = notification.EventId,
                    EventResponse = notification.EventResponse,
                    Model = notification.Model,
                    Module = notification.Module,
                    User = notification.User
                });
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }
        public async Task Handle(RoleDeactivatedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();

                await SB.Publish<IRoleDeactivatedIntegrationEvent, RoleDeactivatedIntegrationEvent>(new RoleDeactivatedIntegrationEvent()
                {
                    App = notification.App,
                    Code = notification.Code,
                    Action = notification.Action,
                    EventCorrelationId = notification.EventCorrelationId,
                    EventCreatedDateTime = notification.EventCreatedDateTime,
                    EventId = notification.EventId,
                    EventResponse = notification.EventResponse,
                    Model = notification.Model,
                    Module = notification.Module,
                    User = notification.User
                });
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }


        public async Task Handle(PrivilegeAssociatedToRoleDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();

                await SB.Publish<IPrivilegeAssociatedToRoleIntegrationEvent, PrivilegeAssociatedToRoleIntegrationEvent>(new PrivilegeAssociatedToRoleIntegrationEvent()
                {
                    App = notification.App,
                    RoleId = notification.RoleId,
                    Privileges = notification.PrivilegeIds,
                    Action = notification.Action,
                    EventCorrelationId = notification.EventCorrelationId,
                    EventCreatedDateTime = notification.EventCreatedDateTime,
                    EventId = notification.EventId,
                    EventResponse = notification.EventResponse,
                    Model = notification.Model,
                    Module = notification.Module,
                    User = notification.User
                });
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }
        public async Task Handle(PrivilegeDeassociatedFromRoleDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();

                await SB.Publish<IPrivilegeDeassociatedFromRoleIntegrationEvent, PrivilegeDeassociatedFromRoleIntegrationEvent>(new PrivilegeDeassociatedFromRoleIntegrationEvent()
                {
                    RoleId = notification.RoleId,
                    Privileges = notification.Privileges,
                    Action = notification.Action,
                    EventCorrelationId = notification.EventCorrelationId,
                    EventCreatedDateTime = notification.EventCreatedDateTime,
                    EventId = notification.EventId,
                    EventResponse = notification.EventResponse,
                    Model = notification.Model,
                    Module = notification.Module,
                    User = notification.User
                });
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }
        public async Task Handle(ClaimsAssociatedToActorDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();

                await SB.Publish<IClaimAssociatedToActorIntegrationEvent, ClaimAssociatedToActorIntegrationEvent>(new ClaimAssociatedToActorIntegrationEvent()
                {
                    App = notification.App,
                    ActorId = notification.ActorId,
                    Claims = notification.Claims,
                    Action = notification.Action,
                    EventCorrelationId = notification.EventCorrelationId,
                    EventCreatedDateTime = notification.EventCreatedDateTime,
                    EventId = notification.EventId,
                    EventResponse = notification.EventResponse,
                    Model = notification.Model,
                    Module = notification.Module,
                    User = notification.User
                });
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }
        public async Task Handle(ClaimsDeassociatedFromActorDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();

                await SB.Publish<IClaimDeAssociatedToActorIntegrationEvent, ClaimDeAssociatedToActorIntegrationEvent>(new ClaimDeAssociatedToActorIntegrationEvent()
                {
                    ActorId = notification.ActorId,
                    Claims = notification.Claims,
                    Action = notification.Action,
                    EventCorrelationId = notification.EventCorrelationId,
                    EventCreatedDateTime = notification.EventCreatedDateTime,
                    EventId = notification.EventId,
                    EventResponse = notification.EventResponse,
                    Model = notification.Model,
                    Module = notification.Module,
                    User = notification.User
                });
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }
        public async Task Handle(ClaimsAssociatedToroleDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();

                await SB.Publish<IClaimAssociatedToRoleIntegrationEvent, ClaimAssociatedToRoleIntegrationEvent>(new ClaimAssociatedToRoleIntegrationEvent()
                {
                    App = notification.App,
                    RoleId = notification.RoleId,
                    Claims = notification.Claims,
                    Action = notification.Action,
                    EventCorrelationId = notification.EventCorrelationId,
                    EventCreatedDateTime = notification.EventCreatedDateTime,
                    EventId = notification.EventId,
                    EventResponse = notification.EventResponse,
                    Model = notification.Model,
                    Module = notification.Module,
                    User = notification.User
                });
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }
        public async Task Handle(ClaimsDeassociatedFromRoleDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                Check();

                await SB.Publish<IClaimDeAssociatedToRoleIntegrationEvent, ClaimDeAssociatedToRoleIntegrationEvent>(new ClaimDeAssociatedToRoleIntegrationEvent()
                {
                    RoleId = notification.RoleId,
                    Claims = notification.Claims,
                    Action = notification.Action,
                    EventCorrelationId = notification.EventCorrelationId,
                    EventCreatedDateTime = notification.EventCreatedDateTime,
                    EventId = notification.EventId,
                    EventResponse = notification.EventResponse,
                    Model = notification.Model,
                    Module = notification.Module,
                    User = notification.User
                });
            }
            catch (Exception x)
            {
                // TODO : log
                //throw;
                return;
            }
        }
        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && (this.SB?.Initialized ?? false);

            return isValid;
        }
        private void Check()
        {
            if (!this.Initialized.GetValueOrDefault())
            {
                throw new Exception("Events Publisher : not initialized correctly.");
            }
        }

        #endregion
    }
}
