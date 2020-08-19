using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XCore.Services.Organizations.Core.Contracts;
using XCore.Services.Organizations.Core.DataLayer.Context;
using XCore.Services.Organizations.Core.DataLayer.Contracts;

namespace XCore.Services.Organizations.Core.DataLayer.Unity
{
    public class OrganizationDataUnity : IOrganizationDataUnity
    {
        #region props

        public bool? Initialized { get; protected set; }
        private readonly OrganizationDataContext _DataContext;

        public IOrganizationRepository Organization { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public ISettingsRepository Settings { get; private set; }
      
        public IOrganizationDelegationRepository OrganizationDelegation { get; private set; }

        public IVenueRepository Venue { get; private set; }
        public ICityRepository city { get; private set; }
        public IRoleRepository Role { get; private set; }
        public IEventRepository Event { get; private set; }

        #endregion
        #region cst.

        public OrganizationDataUnity(OrganizationDataContext dataContext,
                                     IOrganizationRepository organization,
                                     IDepartmentRepository department,
                                     ISettingsRepository settings,
                                     IOrganizationDelegationRepository organizationdelegation,
                                     IVenueRepository venue,
                                     ICityRepository City,
                                     IRoleRepository role,
                                     IEventRepository Event)
                                     


        {
            _DataContext = dataContext;
            Organization = organization;
            Department = department;
            Settings = settings;
            Venue = venue;
            city = City;
            Role = role;
            this.Event = Event;
            OrganizationDelegation = organizationdelegation;
            this.Initialized = Initialize();




        }

        #endregion
        #region publics

        public async Task SaveAsync()
        {
            await _DataContext.SaveChangesAsync();
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this._DataContext != null;
            isValid = isValid && (this.Organization?.Initialized.GetValueOrDefault() ?? false)
                              && (this.Department?.Initialized.GetValueOrDefault() ?? false)
                              && (this.Settings?.Initialized.GetValueOrDefault() ?? false)
                              && (this.OrganizationDelegation?.Initialized.GetValueOrDefault() ?? false)
                              && (this.Venue?.Initialized.GetValueOrDefault() ?? false)
                              && (this.city?.Initialized.GetValueOrDefault() ?? false);

            return isValid;
        }

        #endregion




    }
}
