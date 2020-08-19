using Microsoft.EntityFrameworkCore;
using XCore.Services.Personnel.DataLayer.Context.Configurations.Accounts;
using XCore.Services.Personnel.DataLayer.Context.Configurations.Departments;
using XCore.Services.Personnel.DataLayer.Context.Configurations.Organizations;
using XCore.Services.Personnel.DataLayer.Context.Configurations.Personnels;
using XCore.Services.Personnel.DataLayer.Context.Configurations.Settings;
using XCore.Services.Personnel.Models.Accounts;
using XCore.Services.Personnel.Models.Departments;
using XCore.Services.Personnel.Models.Organizations;
using XCore.Services.Personnel.Models.Personnels;
using XCore.Services.Personnel.Models.Settings;

namespace XCore.Services.Personnel.DataLayer.Context
{
    public class PersonnelDataContext : DbContext
    {
        #region Props.

        #endregion
        #region DbSets

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Person> Personnel { get; set; }
        public virtual DbSet<AccountBase> Accounts { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }

        #endregion

        #region cst.

        public PersonnelDataContext(DbContextOptions<PersonnelDataContext> options) : base(options)
        {
        }

        #endregion
        #region base

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HandleDBConfigurationForDepartment();
            modelBuilder.HandleDBConfigurationForOrganization();
            modelBuilder.HandleDBConfigurationForPersonnel();
            modelBuilder.HandleDBConfigurationForAccount();
            modelBuilder.HandleDBConfigurationForSetting();
        }

        #endregion
    }
}
