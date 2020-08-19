using System.Linq;
using System.Collections.Generic;

using ADS.Tamam.Modules.Integration.Models;
using ADS.Tamam.Modules.Integration.ORM;

namespace ADS.Tamam.Modules.Integration.DataHandlers
{
    public class HolidaysDataHandler
    {
        public List<Holiday> GetIntegrationHolidays()
        {
            using ( var db = new DomainContext() )
            {
                var holidays = db.Holidays.Where(x => x.isSynced == false).ToList();
                return db.CreateDetachedCopy(holidays);
            }
        }

        public void UpdateAsSynced(Holiday holiday)
        {
            using (var db = new DomainContext())
            {
                holiday.isSynced = true;
                db.AttachCopy(holiday);
                db.SaveChanges();
            }
        }
    }
}