using System.Linq;
using System.Collections.Generic;

using ADS.Tamam.Modules.Integration.Models;
using ADS.Tamam.Modules.Integration.ORM;

namespace ADS.Tamam.Modules.Integration.DataHandlers
{
    public class DepartmentsDataHandler
    {
        public List<Department> GetIntegrationDepartments()
        {
            using ( var db = new DomainContext() )
            {
                var departments = db.Departments.Where(x => x.isSynced == false).ToList();
                return db.CreateDetachedCopy(departments);
            }
        }

        public void UpdateAsSynced(Department department)
        {
            using (var db = new DomainContext())
            {
                department.isSynced = true;
                db.AttachCopy(department);
                db.SaveChanges();
            }
        }
    }
}