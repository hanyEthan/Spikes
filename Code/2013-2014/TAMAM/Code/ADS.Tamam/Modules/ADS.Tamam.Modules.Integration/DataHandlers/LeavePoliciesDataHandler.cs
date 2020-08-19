using System.Linq;
using System.Collections.Generic;

using ADS.Tamam.Modules.Integration.Models;
using ADS.Tamam.Modules.Integration.ORM;

namespace ADS.Tamam.Modules.Integration.DataHandlers
{
    public  class LeavePoliciesDataHandler
    {
        public List<LeavePolicy> GetIntegrationLeavePolicies()
        {
            using (var db = new DomainContext())
            {
                var leavePolicies = db.LeavePolicies.Where(x => x.isSynced == false).ToList();
                return db.CreateDetachedCopy(leavePolicies);
            }
        }

        public void UpdateAsSynced(LeavePolicy leavePolicy)
        {
            using (var db = new DomainContext())
            {
                leavePolicy.isSynced = true;
                db.AttachCopy(leavePolicy);
                db.SaveChanges();
            }
        }
    }
}