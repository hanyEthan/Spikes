using System.Linq;
using System.Collections.Generic;

using ADS.Tamam.Modules.Integration.Models;
using ADS.Tamam.Modules.Integration.ORM;

namespace ADS.Tamam.Modules.Integration.DataHandlers
{
    public class GendersDataHandler : IDetailCodeDataHandler
    {
        public List<Gender> GetIntegrationGenders()
        {
            using ( var db = new DomainContext() )
            {
                var genders = db.Genders.Where(x => x.isSynced == false).ToList();
                return db.CreateDetachedCopy(genders);
            }
        }

        public void UpdateAsSynced(object detailCode)
        {
            using (var db = new DomainContext())
            {
                var gender = detailCode as Gender;
                gender.isSynced = true;
                db.AttachCopy(gender);
                db.SaveChanges();
            }
        }

        public List<IDetailCodeSimilar> GetDetailCodes()
        {
            return GetIntegrationGenders().Cast<IDetailCodeSimilar>().ToList();
        }
    }
}
