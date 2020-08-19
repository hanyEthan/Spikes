using System.Collections.Generic;
using ADS.Tamam.Common.Handlers;
using ADS.Tamam.Common.Data.Context;

using ADS.Common.Context;

using ADS.Tamam.Modules.Integration.DataHandlers;
using ADS.Tamam.Modules.Integration.Repositories;
using ADS.Tamam.Modules.Integration.Helpers;

using TamamModels = ADS.Tamam.Common.Data.Model.Domain.Organization;
using IntegrationModels = ADS.Tamam.Modules.Integration.Models;
using ADS.Common.Utilities;

namespace ADS.Tamam.Modules.Integration.Integrators
{
    public class HolidaysIntegrator
    {
        #region fields

        private HolidaysDataHandler holidaysDataHandler;

        public const string CreateKey = "Create Holiday Action";
        public const string EditKey = "Edit Holiday Action";

        #endregion

        #region Props

        private HolidaysDataHandler HolidaysDataHandler
        {
            get
            {
                return holidaysDataHandler ?? (holidaysDataHandler = new HolidaysDataHandler());
            }
        }

        #endregion

        #region publics

        public void Integrate()
        {
            var integrationHolidays = GetIntegrationHolidays();
            if (integrationHolidays.Count == 0) { return; }      
            HolidaysRepository.Reload();
            Integrate(integrationHolidays);         
        }

        private void Integrate(List<IntegrationModels.Holiday> integrationHolidays)
        {
            foreach (var integrationHoliday in integrationHolidays)
            {
                var tamamHoliday = HolidaysRepository.GetHoliday(integrationHoliday.Code);
                if (tamamHoliday == null)
                {
                    // not exist ---> Insert
                    this.Create(integrationHoliday);
                }
                else
                {
                    if (this.IsChanged(tamamHoliday, integrationHoliday))
                    {
                        // exist and Changed ---> update
                        this.Edit(tamamHoliday, integrationHoliday);
                    }
                    else
                    {
                        // exist without changes ---> Do nothing
                        var message = LogHelper.BuildSkippedMessage(integrationHoliday);
                        XLogger.Info(message);
                    }
                }
            }
        }

        #endregion

        #region Helpers

        private bool IsChanged(TamamModels.Holiday tamamHoliday, IntegrationModels.Holiday integrationHoliday)
        {
            if (integrationHoliday.Code != tamamHoliday.Code) return true;
            if (integrationHoliday.Name != tamamHoliday.Name) return true;
            if (integrationHoliday.NameVariant != tamamHoliday.NameCultureVariant) return true;
            if (integrationHoliday.StartDate != tamamHoliday.StartDate) return true;
            if (integrationHoliday.EndDate != tamamHoliday.EndDate) return true;

            return false;
        }

        private List<IntegrationModels.Holiday> GetIntegrationHolidays()
        {
            var integrationHolidays = this.HolidaysDataHandler.GetIntegrationHolidays();
            return integrationHolidays;
        }

        private TamamModels.Holiday Map(TamamModels.Holiday tamamHoliday, IntegrationModels.Holiday integrationHoliday)
        {
            tamamHoliday.Code = integrationHoliday.Code;
            tamamHoliday.Name = integrationHoliday.Name;
            tamamHoliday.NameCultureVariant = integrationHoliday.NameVariant;
            tamamHoliday.StartDate = integrationHoliday.StartDate;
            tamamHoliday.EndDate = integrationHoliday.EndDate;

            return tamamHoliday;
        }

        private void Create(IntegrationModels.Holiday integrationHoliday)
        {
            var newHoliday = Map(new TamamModels.Holiday(), integrationHoliday);
            var response = TamamServiceBroker.OrganizationHandler.CreateNativeHoliday(newHoliday, SystemRequestContext.Instance);
            ResponseHelper.HandleResponse(response, integrationHoliday, CreateKey);
            if (response.Type == ResponseState.Success)
            {
                holidaysDataHandler.UpdateAsSynced(integrationHoliday);
            }
        }

        private void Edit(TamamModels.Holiday tamamHoliday, IntegrationModels.Holiday integrationHoliday)
        {
            var modifiedHoliday = Map(tamamHoliday, integrationHoliday);
            var response = TamamServiceBroker.OrganizationHandler.EditNativeHoliday(modifiedHoliday, SystemRequestContext.Instance);
            ResponseHelper.HandleResponse(response, integrationHoliday, EditKey);
            if (response.Type == ResponseState.Success)
            {
                holidaysDataHandler.UpdateAsSynced(integrationHoliday);
            }
        }
      
        #endregion
    }
}
