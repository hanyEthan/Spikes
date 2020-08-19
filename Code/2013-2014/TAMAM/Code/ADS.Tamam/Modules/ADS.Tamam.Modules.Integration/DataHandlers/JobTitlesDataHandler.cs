using System.Linq;
using System.Collections.Generic;

using ADS.Tamam.Modules.Integration.Models;
using ADS.Tamam.Modules.Integration.ORM;

namespace ADS.Tamam.Modules.Integration.DataHandlers
{
    public class JobTitlesDataHandler : IDetailCodeDataHandler
    {
        public List<JobTitle> GetIntegrationJobTitles()
        {
            using ( var db = new DomainContext() )
            {
                var jobTitles = db.JobTitles.Where(x => x.isSynced == false).ToList();
                return db.CreateDetachedCopy(jobTitles);
            }
        }

        public void UpdateAsSynced(object detailCode)
        {
            using (var db = new DomainContext())
            {
                var jobTitle = detailCode as JobTitle;
                jobTitle.isSynced = true;
                db.AttachCopy(jobTitle);
                db.SaveChanges();
            }
        }

        public List<IDetailCodeSimilar> GetDetailCodes()
        {
            return GetIntegrationJobTitles().Cast<IDetailCodeSimilar>().ToList();
        }
    }

    public interface IDetailCodeDataHandler
    {
        List<IDetailCodeSimilar> GetDetailCodes();
        void UpdateAsSynced(object detailCode);
    }
}
