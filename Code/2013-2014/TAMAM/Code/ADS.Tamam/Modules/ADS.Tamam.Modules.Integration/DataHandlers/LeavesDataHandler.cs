using System.Linq;
using System.Collections.Generic;

using ADS.Tamam.Modules.Integration.ORM;
using ADS.Tamam.Modules.Integration.Models;

namespace ADS.Tamam.Modules.Integration.DataHandlers
{
    public class LeavesDataHandler
    {
        public List<Leave> GetIntegrationLeaves( int take , int skip )
        {
            using ( var db = new DomainContext() )
            {
                var leaves = skip > 0 ? db.Leaves.Where(x => x.isSynced == false).Skip(skip).Take(take).ToList()
                                      : db.Leaves.Where( x => x.isSynced == false ).ToList();
                return db.CreateDetachedCopy(leaves);
            }
        }

        public void UpdateAsSynced(Leave leave)
        {
            using (var db = new DomainContext())
            {
                leave.isSynced = true;
                db.AttachCopy(leave);
                db.SaveChanges();
            }
        }
    }
}
