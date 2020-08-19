using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Context;

using ADS.Common.Context;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data.Model.Domain.Organization;

using ADS.Tamam.Modules.Integration.DataHandlers;
using ADS.Tamam.Modules.Integration.Repositories;
using ADS.Tamam.Common.Data;
using ADS.Tamam.Modules.Integration.Helpers;

using TamamModels = ADS.Tamam.Common.Data.Model.Domain.Personnel;
using IntegrationModels = ADS.Tamam.Modules.Integration.Models;
using ADS.Common.Utilities;

namespace ADS.Tamam.Modules.Integration.Integrators
{
    public class DelegatesIntegrator
    {
        #region fields

        private DelegatesDataHandler delegatesDataHandler;

        public const string CreateKey = "Create Delegate Action";
        public const string EditKey = "Edit Delegate Action";

        #endregion

        #region Props

        private List<TamamModels.Person> TamamPersonnel { get; set; }

        private DelegatesDataHandler DelegatesDataHandler
        {
            get
            {
                return delegatesDataHandler ?? (delegatesDataHandler = new DelegatesDataHandler());
            }
        }

        #endregion

        #region publics

        public void Integrate()
        {
            var round = 0;
            XLogger.Info(LogHelper.BuildMessage("Round {0}", round++));
            IntegrateAllDelegates();
        }

        #endregion

        #region Helpers

        private void IntegrateAllDelegates()
        {
            var skip = 0;
            while (true)
            {
                var integrationDelegates = GetIntegrationDelegates(IntegrationConstants.BatchSize, skip);
                if (integrationDelegates.Count == 0) { break; } else { skip += IntegrationConstants.BatchSize; }
                Integrate(integrationDelegates);

                if ( IntegrationConstants.BatchSize == -1 ) break;
            }
        }

        private void Integrate(List<IntegrationModels.Delegate> integrationDelegates)
        {
            var codes = integrationDelegates.Select(d => d.Code).ToList();
            var tamamDelegates = GetTamamDelegates( codes ) ?? new List<TamamModels.PersonDelegate>();

            var personnelCodes = integrationDelegates.Select(d => d.PersonCode).Distinct().ToList();
            personnelCodes.AddRange(integrationDelegates.Select(d => d.DelegateCode).Distinct().ToList());
            personnelCodes = personnelCodes.Distinct().ToList();
            TamamPersonnel = GetTamamPersonnel(personnelCodes);

            foreach (var integrationDelegate in integrationDelegates)
            {
                var tamamDelegate = tamamDelegates.SingleOrDefault(d => d.Code == integrationDelegate.Code);
                if (tamamDelegate == null)
                {
                    // not exist ---> Insert
                    this.Create(integrationDelegate);
                }
                else
                {
                    if (this.IsChanged(tamamDelegate, integrationDelegate))
                    {
                        // exist and Changed ---> update
                        this.Edit(tamamDelegate, integrationDelegate);
                    }
                    else
                    {
                        // exist without changes ---> Do nothing
                        var message = LogHelper.BuildSkippedMessage(integrationDelegate);
                        XLogger.Info(message);
                    }
                }
            }
        }

        private bool IsChanged(TamamModels.PersonDelegate tamamDelegate, IntegrationModels.Delegate integrationDelegate)
        {
            if (tamamDelegate.Code != integrationDelegate.Code) return true;
            if (tamamDelegate.PersonId != TranslatePerson(integrationDelegate.PersonCode)) return true;
            if (tamamDelegate.DelegateId != TranslatePerson(integrationDelegate.DelegateCode)) return true;
            if (tamamDelegate.StartDate != integrationDelegate.StartDate) return true;
            if (tamamDelegate.EndDate != integrationDelegate.EndDate) return true;

            return false;
        }

        private List<IntegrationModels.Delegate> GetIntegrationDelegates(int take, int skip)
        {
            var integrationDelegates = DelegatesDataHandler.GetIntegrationDelegates(take, skip);
            return integrationDelegates;
        }

        private List<TamamModels.PersonDelegate> GetTamamDelegates(List<string> codes)
        {
            var searchCriteria = new TamamModels.PersonnelDelegatesSearchCriteria() { Codes = codes };
            var TamamPersonnelResponse = TamamServiceBroker.PersonnelHandler.GetPersonnelDelegates(searchCriteria, SystemRequestContext.Instance);
            if (TamamPersonnelResponse.Type != ResponseState.Success) return null;
            return TamamPersonnelResponse.Result.PersonnelDelegates;
        }

        private TamamModels.PersonDelegate Map(TamamModels.PersonDelegate tamamDelegate, IntegrationModels.Delegate integrationDelegate)
        {
            tamamDelegate.Code = integrationDelegate.Code;
            tamamDelegate.DelegateId = TranslatePerson(integrationDelegate.DelegateCode);
            tamamDelegate.PersonId = TranslatePerson(integrationDelegate.PersonCode);
            tamamDelegate.EndDate = integrationDelegate.EndDate;
            tamamDelegate.StartDate = integrationDelegate.StartDate;

            return tamamDelegate;
        }

        private void Create(IntegrationModels.Delegate integrationdelegate)
        {
            var newDelegate = Map(new TamamModels.PersonDelegate(), integrationdelegate);
            var response = TamamServiceBroker.PersonnelHandler.CreateDelegate(newDelegate, SystemRequestContext.Instance);
            ResponseHelper.HandleResponse(response, integrationdelegate, CreateKey);
            UpdateAsSynced(integrationdelegate, response);
        }

        private void Edit(TamamModels.PersonDelegate tamamPerson, IntegrationModels.Delegate integrationDelegate)
        {
            var modifiedDelegate = Map(tamamPerson, integrationDelegate);
            var response = TamamServiceBroker.PersonnelHandler.EditDelegate(modifiedDelegate, SystemRequestContext.Instance);
            ResponseHelper.HandleResponse(response, integrationDelegate, EditKey);
            UpdateAsSynced(integrationDelegate, response);
        }

        private void UpdateAsSynced(IntegrationModels.Delegate integrationDelegate, ExecutionResponse<bool> response)
        {
            if (response.Type == ResponseState.Success)
            {
                DelegatesDataHandler.UpdateAsSynced(integrationDelegate);
            }
        }

        private List<TamamModels.Person> GetTamamPersonnel(List<string> codes)
        {
            var searchCriteria = new TamamModels.PersonSearchCriteria() { Codes = codes };
            var TamamPersonnelResponse = TamamServiceBroker.PersonnelHandler.GetPersonnel(searchCriteria, SystemRequestContext.Instance);
            if (TamamPersonnelResponse.Type != ResponseState.Success) return null;
            return TamamPersonnelResponse.Result.Persons;
        }

        private Guid TranslatePerson(string code)
        {
            var person = TamamPersonnel.SingleOrDefault(m => m.Code == code);
            if (person == null) return new Guid();
            return person.Id;
        }

        #endregion
    }
}
