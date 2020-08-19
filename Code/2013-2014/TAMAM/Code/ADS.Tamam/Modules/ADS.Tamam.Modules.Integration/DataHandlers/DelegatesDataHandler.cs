using System.Linq;
using System.Collections.Generic;

using ADS.Tamam.Modules.Integration.Models;
using ADS.Tamam.Modules.Integration.ORM;

namespace ADS.Tamam.Modules.Integration.DataHandlers
{
    public class DelegatesDataHandler
    {
        public List<Delegate> GetIntegrationDelegates( int take , int skip )
        {
            using ( var db = new DomainContext() )
            {
                var delegates = skip > 0 ? db.Delegates.Where(x => x.isSynced == false).Skip(skip).Take(take).ToList()
                                         : db.Delegates.Where( x => x.isSynced == false ).ToList();
                return db.CreateDetachedCopy(delegates);
            }
        }


        public void UpdateAsSynced(Delegate delegateObj)
        {
            using (var db = new DomainContext())
            {
                delegateObj.isSynced = true;
                db.AttachCopy(delegateObj);
                db.SaveChanges();
            }
        }
    }
}