using System.Linq;
using System.Collections.Generic;

using ADS.Tamam.Modules.Integration.Models;
using ADS.Tamam.Modules.Integration.ORM;

namespace ADS.Tamam.Modules.Integration.DataHandlers
{
    public class NationalitiesDataHandler : IDetailCodeDataHandler
    {
        public List<Nationality> GetIntegrationNationalities()
        {
            using (var db = new DomainContext())
            {
                var nationalities = db.Nationalities.Where(x => x.isSynced == false).ToList();
                return db.CreateDetachedCopy(nationalities);
            }
        }

        public void UpdateAsSynced(object detailCode)
        {
            using (var db = new DomainContext())
            {
                var nationality = detailCode as Nationality;
                nationality.isSynced = true;
                db.AttachCopy(nationality);
                db.SaveChanges();
            }
        }
        public List<IDetailCodeSimilar> GetDetailCodes()
        {
            return GetIntegrationNationalities().Cast<IDetailCodeSimilar>().ToList();
        }
    }
}
