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

using TamamModels = ADS.Tamam.Common.Data.Model.Domain.Organization;
using IntegrationModels = ADS.Tamam.Modules.Integration.Models;
using ADS.Common.Utilities;

namespace ADS.Tamam.Modules.Integration.Integrators
{
    public class DepartmentsIntegrator
    {
        #region fields

        private DepartmentsDataHandler departmentDataHandler;

        public const string CreateKey = "Create Department Action";
        public const string EditKey = "Edit Department Action";

        private List<IntegrationModels.Department> failedDepartments;

        #endregion

        #region Props

        private DepartmentsDataHandler DepartmentDataHandler
        {
            get
            {
                return departmentDataHandler ?? (departmentDataHandler = new DepartmentsDataHandler());
            }
        }

        private List<IntegrationModels.Department> FailedDepartments
        {
            get
            {
                return failedDepartments ?? (failedDepartments= new List<IntegrationModels.Department>()); ;
            }
            set
            {
                failedDepartments= value;
            }
        }

        #endregion

        #region publics

        public void Integrate()
        {     
            var integrationDepartments = GetIntegrationDepartments();
            if (integrationDepartments.Count == 0) { return; }      
            DepartmentsRepository.Reload();
       
            var round = 0;
            while (round <= IntegrationConstants.Retries && integrationDepartments.Count > 0)
            {
                XLogger.Info(LogHelper.BuildMessage("Round {0}", round));
                DepartmentsRepository.Reload();
                round++;
                FailedDepartments.Clear();
                Integrate(integrationDepartments);
                integrationDepartments = new List<IntegrationModels.Department>(FailedDepartments);
            }
        }

        private void Integrate(List<IntegrationModels.Department> integrationDepartments)
        {
            foreach (var integrationDepartment in integrationDepartments)
            {
                var tamamDepartment = DepartmentsRepository.GetDepartment(integrationDepartment.Code);
                if (tamamDepartment == null)
                {
                    // not exist ---> Insert
                    this.Create(integrationDepartment);
                }
                else
                {
                    if (this.IsChanged(tamamDepartment, integrationDepartment))
                    {
                        // exist and Changed ---> update
                        this.Edit(tamamDepartment, integrationDepartment);
                    }
                    else
                    {
                        // exist without changes ---> Do nothing
                        var message = LogHelper.BuildSkippedMessage(integrationDepartment);
                        XLogger.Info(message);
                    }
                }
            }
        }

        #endregion

        #region Helpers

        private bool IsChanged(TamamModels.Department tamamDepartment, IntegrationModels.Department integrationDepartment)
        {
            if (integrationDepartment.Code != tamamDepartment.Code) return true;
            if (integrationDepartment.Name != tamamDepartment.Name) return true;

            if (integrationDepartment.NameVariant != tamamDepartment.NameCultureVarient) return true;
            if (DepartmentsRepository.Translate(integrationDepartment.ParentCode) != tamamDepartment.ParentDepartmentId) return true;

            return false;
        }

        private List<IntegrationModels.Department> GetIntegrationDepartments()
        {
            var integrationDepartments = DepartmentDataHandler.GetIntegrationDepartments();
            return integrationDepartments;
        }

        private TamamModels.Department Map(TamamModels.Department tamamDepartment, IntegrationModels.Department integrationDepartment)
        {
            tamamDepartment.Code = integrationDepartment.Code;
            tamamDepartment.Name = integrationDepartment.Name;
            tamamDepartment.NameCultureVarient = integrationDepartment.NameVariant;
            tamamDepartment.ParentDepartmentId = DepartmentsRepository.Translate(integrationDepartment.ParentCode);

            return tamamDepartment;
        }

        private void Create(IntegrationModels.Department integrationDepartment)
        {
            var newDepartment = Map(new TamamModels.Department(), integrationDepartment);
            var response = TamamServiceBroker.OrganizationHandler.CreateDepartment(newDepartment, SystemRequestContext.Instance);
            ResponseHelper.HandleResponse(response, integrationDepartment, CreateKey);
            UpdateBaseOnResponse(integrationDepartment, response);
        }

        private void Edit(TamamModels.Department tamamDepartment, IntegrationModels.Department integrationDepartment)
        {
            var modifiedDepartment = Map(tamamDepartment, integrationDepartment);
            var response = TamamServiceBroker.OrganizationHandler.EditDepartment(modifiedDepartment, SystemRequestContext.Instance);
            ResponseHelper.HandleResponse(response, integrationDepartment, EditKey);
            UpdateBaseOnResponse(integrationDepartment, response);
        }

        private void UpdateBaseOnResponse<T>(IntegrationModels.Department integrationDepartment, ExecutionResponse<T> response) where T : new()
        {
            if (response.Type == ResponseState.Success)
            {
                departmentDataHandler.UpdateAsSynced(integrationDepartment);                
            }
            else
            {
                FailedDepartments.Add(integrationDepartment);
            }
        }

        #endregion
    }
}
