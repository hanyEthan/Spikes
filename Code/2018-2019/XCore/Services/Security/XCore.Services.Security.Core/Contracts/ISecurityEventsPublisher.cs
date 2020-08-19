using System.Threading.Tasks;
using XCore.Services.Security.Core.Models.Events.Domain;

namespace XCore.Services.Security.Core.Contracts
{
    public interface ISecurityEventsPublisher
    {
        bool? Initialized { get; }

        Task RoleCreatedEvent(RoleCreatedDomainEvent Event);
        Task RoleDeletedEvent(RoleDeletedDomainEvent Event);
        Task RoleActivatedEvent(RoleActivatedDomainEvent Event);
        Task RoleDeactivatedEvent(RoleDeactivatedDomainEvent Event);

        Task RolesAssociatedToActorEvent(RolesAssociatedToActorDomainEvent Event);
        Task RoleDeassociatedFromActorEvent(RolesDeassociatedFromActorDomainEvent Event);

        Task PrivilegeAssociatedToRoleEvent(PrivilegeAssociatedToRoleDomainEvent Event);
        Task PrivilegeDeassociatedFromRoleEvent(PrivilegeDeassociatedFromRoleDomainEvent Event);

        Task PrivilegeAssociatedToActorEvent(PrivilegeAssociatedToActorDomainEvent Event);
        Task PrivilegeDeassociatedFromActorEvent(PrivilegeDeassociatedFromActorDomainEvent Event);
    }
}
