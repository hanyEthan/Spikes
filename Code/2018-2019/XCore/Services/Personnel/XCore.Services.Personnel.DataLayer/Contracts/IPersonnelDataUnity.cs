using System.Threading.Tasks;
using XCore.Services.Personnel.DataLayer.Contracts.Accounts;
using XCore.Services.Personnel.DataLayer.Contracts.Departments;
using XCore.Services.Personnel.DataLayer.Contracts.Organizations;
using XCore.Services.Personnel.DataLayer.Contracts.Personnels;
using XCore.Services.Personnel.DataLayer.Contracts.Settings;

namespace XCore.Services.Personnel.DataLayer.Contracts
{
    public interface IPersonnelDataUnity
    {
        bool? Initialized { get; }
        IPersonnelRepository Personnel { get; }
        IDepartmentRepository Department { get; }
        IOrganizationRepository Organization { get; }
        IPersonnelAccountRepository PersonnelAccount { get; }
        IOrganizationAccountRepository OrganizationAccount { get; }
        ISettingRepository Setting { get; }
        Task SaveAsync();
    }
}