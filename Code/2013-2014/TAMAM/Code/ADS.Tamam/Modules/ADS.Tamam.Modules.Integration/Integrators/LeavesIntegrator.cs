using ADS.Tamam.Modules.Integration.DataHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADS.Tamam.Modules.Integration.Helpers;

using TamamModels = ADS.Tamam.Common.Data.Model.Domain.Leaves;
using IntegrationModels = ADS.Tamam.Modules.Integration.Models;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Context;
using ADS.Common.Context;
using ADS.Tamam.Common.Data.Model.Domain.Personnel;
using ADS.Tamam.Modules.Integration.Repositories;
using ADS.Common.Utilities;
using ADS.Tamam.Common.Data.Model.Enums;
using ADS.Tamam.Common.Data;

namespace ADS.Tamam.Modules.Integration.Integrators
{
    public class LeavesIntegrator
    {

        #region fields

        private LeavesDataHandler leavesDataHandler;

        public const string CreateKey = "Create Leave Action";
        public const string EditKey = "Edit Leave Action";

        #endregion

        #region Props

        private LeavesDataHandler LeavesDataHandler
        {
            get
            {
                return leavesDataHandler ?? (leavesDataHandler = new LeavesDataHandler());
            }
        }

        private List<Person> TamamPersonnel { get; set; }

        private DetailCodeRepository LeaveStatusRepository { get; set; }
        private DetailCodeRepository LeaveTypesRepository { get; set; }

        #endregion

        #region Ctor

        public LeavesIntegrator()
        {
            LeaveStatusRepository = new DetailCodeRepository(TamamConstants.MasterCodes.LeaveStatus);
            LeaveTypesRepository = new DetailCodeRepository(TamamConstants.MasterCodes.LeaveType);
        }

        #endregion

        #region publics

        public void Integrate()
        {
            var round = 0;
            XLogger.Info(LogHelper.BuildMessage("Round {0}", round++));
            IntegrateAllLeaves();
        }

        #endregion

        #region Helpers

        private void IntegrateAllLeaves()
        {
            var skip = 0;
            while (true)
            {
                var integrationLeaves = GetIntegrationLeaves(IntegrationConstants.BatchSize, skip);
                if (integrationLeaves.Count == 0) { break; } else { skip += IntegrationConstants.BatchSize; }
                Integrate(integrationLeaves);

                if ( IntegrationConstants.BatchSize == -1 ) break;
            }
        }

        private void Integrate(List<IntegrationModels.Leave> integrationLeaves)
        {
            var leavesCodes = integrationLeaves.Select(p => p.Code).ToList();
            var tamamLeaves = GetTamamLeaves(leavesCodes);

            var personnelCodes = integrationLeaves.Select(p => p.PersonCode).ToList();
            TamamPersonnel = GetTamamPersonnel(personnelCodes);

            foreach (var integrationLeave in integrationLeaves)
            {
                var tamamLeave = tamamLeaves.SingleOrDefault(l => l.Code == integrationLeave.Code);
                if (tamamLeave == null)
                {
                    // not exist ---> Insert
                    this.Create(integrationLeave);
                }
                else
                {
                    if (this.IsChanged(tamamLeave, integrationLeave))
                    {
                        // exist and Changed ---> update
                        this.Edit(tamamLeave, integrationLeave);
                    }
                    else
                    {
                        // exist without changes ---> Do nothing
                        var message = LogHelper.BuildSkippedMessage(integrationLeave);
                        XLogger.Info(message);
                    }
                }
            }
        }

        private bool IsChanged(TamamModels.Leave tamamLeave, IntegrationModels.Leave integrationLeave)
        {
            if (integrationLeave.Code != tamamLeave.Code) return true;
            if (integrationLeave.StartDate != tamamLeave.StartDate) return true;
            if (integrationLeave.EndDate != tamamLeave.EndDate) return true;

            if (tamamLeave.Person == null) return true;
            if (integrationLeave.PersonCode != tamamLeave.Person.Code) return true;

            if (tamamLeave.LeaveStatus == null) return true;
            if (integrationLeave.StatusCode != tamamLeave.LeaveStatus.Code) return true;

            if (tamamLeave.LeaveType == null) return true;
            if (integrationLeave.TypeCode != tamamLeave.LeaveType.Code) return true;

            if (integrationLeave.Notes != tamamLeave.Notes) return true;

            return false;
        }

        private List<IntegrationModels.Leave> GetIntegrationLeaves(int take, int skip)
        {
            var integrationLeaves = LeavesDataHandler.GetIntegrationLeaves(take, skip);
            return integrationLeaves;
        }

        private List<TamamModels.Leave> GetTamamLeaves(List<string> codes)
        {
            var searchCriteria = new TamamModels.LeaveSearchCriteria() { Codes = codes };
            var activePersonnelOnly = false;
            var TamamPersonnelResponse = TamamServiceBroker.LeavesHandler.SearchLeaves(searchCriteria, activePersonnelOnly, SystemRequestContext.Instance);
            if (TamamPersonnelResponse.Type != ResponseState.Success) return null;
            return TamamPersonnelResponse.Result.Leaves;
        }

        private TamamModels.Leave Map(TamamModels.Leave tamamLeave, IntegrationModels.Leave integrationLeave)
        {
            tamamLeave.Code = integrationLeave.Code;
            tamamLeave.StartDate = integrationLeave.StartDate;
            tamamLeave.EndDate = integrationLeave.EndDate;

            var personId = TranslatePerson(integrationLeave.PersonCode);
            if (personId == null) throw new ApplicationException(string.Format("Person [{0}] doesn't Exist", integrationLeave.PersonCode));
            tamamLeave.PersonId = personId.Value;

            tamamLeave.LeaveStatusId = LeaveStatusRepository.Translate(integrationLeave.StatusCode);
            tamamLeave.LeaveTypeId = LeaveTypesRepository.Translate(integrationLeave.TypeCode);
            tamamLeave.Notes = integrationLeave.Notes;

            return tamamLeave;
        }

        private void Create(IntegrationModels.Leave integrationLeave)
        {
            try
            {
                var newLeave = Map(new TamamModels.Leave(), integrationLeave);
                var response = SystemBroker.LeavesHandler.CreateLeaves(new List<TamamModels.Leave>() { newLeave }, true, SystemRequestContext.Instance);
                ResponseHelper.HandleResponse(response, integrationLeave, CreateKey);
                UpdateAsSynced(integrationLeave, response);
            }
            catch (Exception e)
            {
                XLogger.Error(LogHelper.BuildSkippedMessage(integrationLeave, e.Message));
            }

        }

        private void Edit(TamamModels.Leave tamamLeave, IntegrationModels.Leave integrationLeave)
        {
            try
            {
                var modifiedLeave = Map(tamamLeave, integrationLeave);
                var response = SystemBroker.LeavesHandler.EditLeave(modifiedLeave, true, SystemRequestContext.Instance);
                ResponseHelper.HandleResponse(response, integrationLeave, EditKey);
                UpdateAsSynced(integrationLeave, response);
            }
            catch (Exception e)
            {
                XLogger.Error(LogHelper.BuildSkippedMessage(integrationLeave, e.Message));
            }
        }

        private List<Person> GetTamamPersonnel(List<string> codes)
        {
            var searchCriteria = new PersonSearchCriteria() { Codes = codes };
            var TamamPersonnelResponse = TamamServiceBroker.PersonnelHandler.GetPersonnel(searchCriteria, SystemRequestContext.Instance);
            if (TamamPersonnelResponse.Type != ResponseState.Success) return null;
            return TamamPersonnelResponse.Result.Persons;
        }

        private Guid? TranslatePerson(string code)
        {
            var person = TamamPersonnel.SingleOrDefault(p => p.Code == code);
            if (person == null) return null;
            return person.Id;
        }

        private void UpdateAsSynced(IntegrationModels.Leave integrationLeave, ExecutionResponse<bool> response)
        {
            if (response.Type == ResponseState.Success)
            {
                leavesDataHandler.UpdateAsSynced(integrationLeave);
            }
        }

        #endregion
    }
}