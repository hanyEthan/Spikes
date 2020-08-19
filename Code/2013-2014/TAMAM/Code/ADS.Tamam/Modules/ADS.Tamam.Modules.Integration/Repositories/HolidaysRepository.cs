using ADS.Common.Context;
using ADS.Common.Handlers;
using ADS.Common.Models.Domain;
using ADS.Tamam.Common.Data;
using ADS.Tamam.Common.Data.Context;
using ADS.Tamam.Common.Data.Model.Domain.Organization;
using ADS.Tamam.Common.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Modules.Integration.Repositories
{
    public static class HolidaysRepository
    {
        #region fields

        private static List<Holiday> holidays;

        #endregion

        #region props

        private static List<Holiday> Holidays
        {
            get
            {
                if (holidays == null) Reload();
                return holidays;
            }
            set
            {
                holidays = value;
            }
        }

        #endregion

        #region helpers

        private static List<Holiday> GetHolidays()
        {
            var TamamHolidaysResponse = TamamServiceBroker.OrganizationHandler.GetHolidays(SystemRequestContext.Instance);
            if (TamamHolidaysResponse.Type != ResponseState.Success) return null;
            return TamamHolidaysResponse.Result;
        }

        #endregion

        #region publics

        public static Holiday GetHoliday(string code)
        {
            var holiday = Holidays.SingleOrDefault(g => g.Code == code);
            return holiday;
        }

        public static Guid Translate(string code)
        {
            var holiday = Holidays.SingleOrDefault(g => g.Code == code);
            return holiday == null ? Guid.Empty : holiday.Id;
        }

        public static void Reload()
        {
            Holidays = GetHolidays();
        }

        #endregion
    }
}
