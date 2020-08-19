using Microsoft.EntityFrameworkCore;
using XCore.Services.Hiring.Core.DataLayer.Context.Configurations;

namespace XCore.Services.Hiring.Core.DataLayer.Context
{
    public class HiringDataContext : DbContext
    {            
        #region cst.

        public HiringDataContext(DbContextOptions<HiringDataContext> options) : base(options)
        {
        }

        #endregion
        #region base

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HandleAdvertisementsDBConfigurations();
            modelBuilder.HandleCandidatesDBConfigurations();
            modelBuilder.HandleAnswersDBConfigurations();
            modelBuilder.HandleApplicationsDBConfigurations();
            modelBuilder.HandleHiringProcessesDBConfigurations();
            modelBuilder.HandleHiringStepsDBConfigurations();
            modelBuilder.HandleOrganizationsDBConfigurations();
            modelBuilder.HandlePersonsOfInterestDBConfigurations();
            modelBuilder.HandlePositionsDBConfigurations();
            modelBuilder.HandleQuestionsDBConfigurations();
            modelBuilder.HandleRolesDBConfigurations();
            modelBuilder.HandleSkillsDBConfigurations();
            modelBuilder.HandleAdvertisementsSkillsDBConfigurations();
        }
        #endregion
    }
}
