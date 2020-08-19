using System.Threading.Tasks;
using XCore.Services.Organizations.Core.DataLayer.Contracts;

namespace XCore.Services.Organizations.Core.Contracts
{
    public interface IOrganizationDataUnity
    {
        bool? Initialized { get; }

        IOrganizationRepository Organization { get; }
        IDepartmentRepository Department { get; }
        ISettingsRepository Settings { get; }
        IOrganizationDelegationRepository OrganizationDelegation { get; }
        IVenueRepository Venue { get; }
        ICityRepository city { get; }
        IRoleRepository Role { get; }
        IEventRepository Event { get;  }

        Task SaveAsync();
    }
}
