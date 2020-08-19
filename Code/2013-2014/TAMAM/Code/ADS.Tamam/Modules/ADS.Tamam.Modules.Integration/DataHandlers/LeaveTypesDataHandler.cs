using System.Linq;
using System.Collections.Generic;

using ADS.Tamam.Modules.Integration.Models;
using ADS.Tamam.Modules.Integration.ORM;

namespace ADS.Tamam.Modules.Integration.DataHandlers
{
    public class LeaveTypesDataHandler : IDetailCodeDataHandler
    {
        public List<LeaveType> GetIntegrationLeaveTypes()
        {
            using (var db = new DomainContext())
            {
                var leaveTypes = db.LeaveTypes.Where(x => x.isSynced == false).ToList();
                return db.CreateDetachedCopy(leaveTypes);
            }
        }

        public void UpdateAsSynced(object detailCode)
        {
            using (var db = new DomainContext())
            {
                var leaveType = detailCode as LeaveType;
                leaveType.isSynced = true;
                db.AttachCopy(leaveType);
                db.SaveChanges();
            }
        }

        public List<IDetailCodeSimilar> GetDetailCodes()
        {
            return GetIntegrationLeaveTypes().Cast<IDetailCodeSimilar>().ToList();
        }
    }
}
