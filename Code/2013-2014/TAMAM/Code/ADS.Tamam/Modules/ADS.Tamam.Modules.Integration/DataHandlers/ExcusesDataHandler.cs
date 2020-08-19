using System.Linq;
using System.Collections.Generic;

using ADS.Tamam.Modules.Integration.ORM;
using ADS.Tamam.Modules.Integration.Models;

namespace ADS.Tamam.Modules.Integration.DataHandlers
{
   public class ExcusesDataHandler
    {
        public List<Excuse> GetIntegrationExcuses( int take , int skip )
        {
            using ( var db = new DomainContext() )
            {
                var excuses = skip > 0 ? db.Excuses.Where(x => x.isSynced == false).Skip(skip).Take(take).ToList()
                    : db.Excuses.Where( x => x.isSynced == false ).ToList();
                return db.CreateDetachedCopy(excuses);
            }
        }

        public void UpdateAsSynced(Excuse excuse)
        {
            using (var db = new DomainContext())
            {
                excuse.isSynced = true;
                db.AttachCopy(excuse);
                db.SaveChanges();
            }
        }
    }
}
