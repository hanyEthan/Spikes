using ADS.Tamam.Modules.Integration.Models;
using ADS.Tamam.Modules.Integration.ORM;
using System.Collections.Generic;
using System.Linq;

namespace ADS.Tamam.Modules.Integration.DataHandlers
{
    public class ReligionsDataHandler : IDetailCodeDataHandler
    {
        public List<Religion> GetIntegrationRelegions()
        {
            using (var db = new DomainContext())
            {
                var religions = db.Religions.Where(x => x.isSynced == false).ToList();
                return db.CreateDetachedCopy(religions);
            }
        }

        public void UpdateAsSynced(object detailCode)
        {
            using (var db = new DomainContext())
            {
                var religion = detailCode as Religion;
                religion.isSynced = true;
                db.AttachCopy(religion);
                db.SaveChanges();
            }
        }

        public List<IDetailCodeSimilar> GetDetailCodes()
        {
            return GetIntegrationRelegions().Cast<IDetailCodeSimilar>().ToList();
        }
    }
}
