using System.Collections.Generic;
using System.Linq;
using ADS.Tamam.Modules.Integration.Models;
using ADS.Tamam.Modules.Integration.ORM;

namespace ADS.Tamam.Modules.Integration.DataHandlers
{
    public class PersonnelDataHandler
    {
        public List<Person> GetIntegrationPersonnel(int take, int skip)
        {
            using (var db = new DomainContext())
            {
                var personnel = skip > 0 ? db.Personnel.Where( x => x.isSynced == false ).Skip( skip ).Take( take ).ToList()
                                         : db.Personnel.Where( x => x.isSynced == false ).ToList();
                return db.CreateDetachedCopy(personnel);
            }
        }

        public void UpdateAsSynced(Person person)
        {
            using (var db = new DomainContext())
            {
                person.isSynced = true;
                db.AttachCopy(person);
                db.SaveChanges();
            }
        }
    }
}