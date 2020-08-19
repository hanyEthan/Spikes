using ADS.Tamam.Modules.Integration.DataHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using ADS.Tamam.Modules.Integration.Helpers;

using TamamModels = ADS.Tamam.Common.Data.Model.Domain.Leaves;
using IntegrationModels = ADS.Tamam.Modules.Integration.Models;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Context;
using ADS.Common.Context;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Modules.Integration.Repositories;
using ADS.Common.Utilities;

namespace ADS.Tamam.Modules.Integration.Integrators
{
    public class ExcusesIntegrator
    {
        #region fields

        private ExcusesDataHandler leavesDataHandler;

        public const string CreateKey = "Create Excuse Action";
        public const string EditKey = "Edit Excuse Action";

        #endregion

        #region Props

        private ExcusesDataHandler ExcusesDataHandler
        {
            get
            {
                return leavesDataHandler ?? (leavesDataHandler = new ExcusesDataHandler());
            }
        }
        
        private List<Person> TamamPersonnel { get; set; }
      
        #endregion

        #region publics

        public void Integrate()
        {
            var round = 0;
            XLogger.Info(LogHelper.BuildMessage("Round {0}", round++));
            IntegrateAllExcuses();
        }

        #endregion

        #region Helpers

        private void IntegrateAllExcuses()
        {
            var skip = 0;
            while (true)
            {
                var integrationExcuses = GetIntegrationExcuses(IntegrationConstants.BatchSize, skip);
                if (integrationExcuses.Count == 0) { break; } else { skip += IntegrationConstants.BatchSize; }
                Integrate(integrationExcuses);

                if ( IntegrationConstants.BatchSize == -1 ) break;
            }
        }

        private void Integrate(List<IntegrationModels.Excuse> integrationExcuses)
        {
            var excusesCodes = integrationExcuses.Select(e => e.Code).ToList();
            var tamamExcuses = GetTamamExcuses(excusesCodes);

            var personnelCodes = integrationExcuses.Select(p => p.PersonCode).ToList();
            TamamPersonnel = GetTamamPersonnel(personnelCodes);

            foreach (var integrationExcuse in integrationExcuses)
            {
                var tamamExcuse = tamamExcuses.SingleOrDefault(l => l.Code == integrationExcuse.Code);
                if (tamamExcuse == null)
                {
                    // not exist ---> Insert
                    this.Create(integrationExcuse);
                }
                else
                {
                    if (this.IsChanged(tamamExcuse, integrationExcuse))
                    {
                        // exist and Changed ---> update
                        this.Edit(tamamExcuse, integrationExcuse);
                    }
                    else
                    {
                        // exist without changes ---> Do nothing
                        var message = LogHelper.BuildSkippedMessage(integrationExcuse);
                        XLogger.Info(message);
                    }
                }
            }
        }

        private bool IsChanged(TamamModels.Excuse tamamExcuse, IntegrationModels.Excuse integrationExcuse)
        {
            if (integrationExcuse.Code != tamamExcuse.Code) return true;
            if (integrationExcuse.StartTime != tamamExcuse.StartTime) return true;
            if (integrationExcuse.EndTime!= tamamExcuse.EndTime) return true;

            if (tamamExcuse.Person == null) return true;
            if (integrationExcuse.PersonCode != tamamExcuse.Person.Code) return true;

            if (tamamExcuse.ExcuseStatus == null) return true; 
            if (integrationExcuse.StatusCode != tamamExcuse.ExcuseStatus.Code) return true;

            if (tamamExcuse.ExcuseType == null) return true;
            if (integrationExcuse.TypeCode != tamamExcuse.ExcuseType.Code) return true;

            if (integrationExcuse.Notes != tamamExcuse.Notes) return true;
            
            return false;
        }

        private List<IntegrationModels.Excuse> GetIntegrationExcuses(int take, int skip)
        {
            var integrationExcuses = ExcusesDataHandler.GetIntegrationExcuses(take, skip);
            return integrationExcuses;
        }

        private List<TamamModels.Excuse> GetTamamExcuses(List<string> codes)
        {
            var searchCriteria = new TamamModels.ExcuseSearchCriteria(null, codes, null, null, null, null, null, false, 0, 0);
            var TamamPersonnelResponse = TamamServiceBroker.LeavesHandler.SearchExcuses(searchCriteria, SystemRequestContext.Instance);
            if (TamamPersonnelResponse.Type != ResponseState.Success) return null;
            return TamamPersonnelResponse.Result.Excuses;
        }

        private TamamModels.Excuse Map(TamamModels.Excuse tamamExcuse, IntegrationModels.Excuse integrationExcuse)
        {
            tamamExcuse.Code = integrationExcuse.Code;
            tamamExcuse.ExcuseDate= integrationExcuse.StartTime.Date;
            tamamExcuse.StartTime = integrationExcuse.StartTime;
            tamamExcuse.EndTime= integrationExcuse.EndTime;
            tamamExcuse.PersonId = TranslatePerson(integrationExcuse.PersonCode);
            tamamExcuse.ExcuseStatusId = ExcuseStatusRepository.Translate(integrationExcuse.StatusCode);
            tamamExcuse.ExcuseTypeId = ExcuseTypesRepository.Translate(integrationExcuse.TypeCode);
            tamamExcuse.Notes = integrationExcuse.Notes;

            return tamamExcuse;
        }

        private void Create(IntegrationModels.Excuse integrationExcuse)
        {
            var newExcuse = Map(new TamamModels.Excuse(), integrationExcuse);
            var response = SystemBroker.LeavesHandler.CreateExcuses(new List<TamamModels.Excuse>() { newExcuse }, true, SystemRequestContext.Instance);
            ResponseHelper.HandleResponse(response, integrationExcuse, CreateKey);
            UpdateAsSynced(integrationExcuse, response);
        }
       
        private void Edit(TamamModels.Excuse tamamExcuse, IntegrationModels.Excuse integrationExcuse)
        {
            var modifiedExcuse= Map(tamamExcuse, integrationExcuse);
            var response = SystemBroker.LeavesHandler.EditExcuse(modifiedExcuse, true, SystemRequestContext.Instance);
            ResponseHelper.HandleResponse(response, integrationExcuse, EditKey);
            UpdateAsSynced(integrationExcuse, response);
        }

        private void UpdateAsSynced(IntegrationModels.Excuse integrationExcuse, ExecutionResponse<bool> response)
        {
            if (response.Type == ResponseState.Success)
            {
                ExcusesDataHandler.UpdateAsSynced(integrationExcuse);
            }
        }

        private List<Person> GetTamamPersonnel(List<string> codes)
        {
            var searchCriteria = new PersonSearchCriteria() { Codes = codes };
            var TamamPersonnelResponse = TamamServiceBroker.PersonnelHandler.GetPersonnel(searchCriteria, SystemRequestContext.Instance);
            if (TamamPersonnelResponse.Type != ResponseState.Success) return null;
            return TamamPersonnelResponse.Result.Persons;
        }

        private Guid TranslatePerson(string code)
        {
            var person = TamamPersonnel.SingleOrDefault(p => p.Code == code);
            if (person == null) return Guid.Empty;
            return person.Id;
        }

        #endregion
    }
}
