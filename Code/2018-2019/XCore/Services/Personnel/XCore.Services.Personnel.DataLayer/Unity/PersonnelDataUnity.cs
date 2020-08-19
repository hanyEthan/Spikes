using System.Threading.Tasks;
using XCore.Services.Personnel.DataLayer.Context;
using XCore.Services.Personnel.DataLayer.Contracts;
using XCore.Services.Personnel.DataLayer.Contracts.Accounts;
using XCore.Services.Personnel.DataLayer.Contracts.Departments;
using XCore.Services.Personnel.DataLayer.Contracts.Organizations;
using XCore.Services.Personnel.DataLayer.Contracts.Personnels;
using XCore.Services.Personnel.DataLayer.Contracts.Settings;

namespace XCore.Services.Personnel.DataLayer.Unity
{
    public class PersonnelDataUnity : IPersonnelDataUnity
    {

        #region props

        public bool? Initialized { get; protected set; }
        private readonly PersonnelDataContext _DataContext;

        public IPersonnelRepository Personnel { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public IOrganizationRepository Organization { get; private set; }
        public IPersonnelAccountRepository PersonnelAccount { get; private set; }
        public IOrganizationAccountRepository OrganizationAccount { get; private set; }
        public ISettingRepository Setting { get; private set; }

        #endregion
        #region cst.

        public PersonnelDataUnity(PersonnelDataContext dataContext, IPersonnelRepository PersonnelRepository,
            IDepartmentRepository DepartmentRepository, IOrganizationRepository OrganizationRepository,
            IPersonnelAccountRepository PersonalAccountRepository, IOrganizationAccountRepository OrganizationAccountRepository,
            ISettingRepository SettingRepository)
        {
            this._DataContext = dataContext;
            this.Personnel = PersonnelRepository;
            this.Department = DepartmentRepository;
            this.Organization = OrganizationRepository;
            this.PersonnelAccount = PersonalAccountRepository;
            this.OrganizationAccount = OrganizationAccountRepository;
            this.Setting = SettingRepository;
            this.Initialized = Initialize();
        }

        #endregion

        #region publics

        public async Task SaveAsync()
        {
            await this._DataContext.SaveChangesAsync();
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this._DataContext != null;
            isValid = isValid && (this.Personnel?.Initialized.GetValueOrDefault() ?? false);

            return isValid;
        }

        #endregion
    }
}
