using System.Linq;
using System.Collections.Generic;

using ADS.Tamam.Modules.Integration.Models;
using ADS.Tamam.Modules.Integration.ORM;

namespace ADS.Tamam.Modules.Integration.DataHandlers
{
    public class MaritalStatusesDataHandler : IDetailCodeDataHandler
    {
        public List<MaritalStatus> GetIntegrationMaritalStatuses()
        {
            using (var db = new DomainContext())
            {
                var maritalStatuses = db.MaritalStatuses.Where(x => x.isSynced == false).ToList();
                return db.CreateDetachedCopy(maritalStatuses);
            }
        }

        public void UpdateAsSynced(object detailCode)
        {
            using (var db = new DomainContext())
            {
                var maritalStatus = detailCode as MaritalStatus;
                maritalStatus.isSynced = true;
                db.AttachCopy(maritalStatus);
                db.SaveChanges();
            }
        }

        public List<IDetailCodeSimilar> GetDetailCodes()
        {
            return GetIntegrationMaritalStatuses().Cast<IDetailCodeSimilar>().ToList();
        }
    }
}
