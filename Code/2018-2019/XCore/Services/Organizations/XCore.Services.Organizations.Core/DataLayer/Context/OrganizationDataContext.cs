using Microsoft.EntityFrameworkCore;
using XCore.Services.Organizations.Core.Models.Domain;
using XCore.Services.Organizations.Core.Models;
using XCore.Services.Organizations.Core.DataLayer.Context.Configurations;

namespace XCore.Services.Organizations.Core.DataLayer.Context
{
    public class OrganizationDataContext : DbContext
    {
        #region Props.

        #endregion
        #region DbSets

        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<OrganizationDelegation> OrganizationDelegation { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Venue> Venue { get; set; }
        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<VenueCity> VenueCity { get; set; }
        public virtual DbSet<Settings> Settings { get; set; }
        public virtual DbSet<ContactInfo> ContactInfo { get; set; }
        public virtual DbSet<ContactPerson> ContactPersonal { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<VenueDepartment> VenueDepartment { get; set; }


        #endregion
        #region cst.

        public OrganizationDataContext(DbContextOptions<OrganizationDataContext> options) : base(options)
        {
        }

        #endregion
        #region base

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Call Extention Methods which Contains Configuation
            modelBuilder.HandleDBConfigurationForOrganization();
            modelBuilder.HandleDBConfigurationForDepartment();
            modelBuilder.HandleDBConfigurationForContactInfo();
            modelBuilder.HandleDBConfigurationForContactPersonal();
            modelBuilder.HandleOrganizationDelegationConfiguration();
            modelBuilder.HandleDBConfigurationForVenue();
            modelBuilder.HandleDBConfigurationForCity();
            modelBuilder.HandleDBConfigurationForEvent();
            modelBuilder.HandleDBConfigurationForVenueDepartment();
            modelBuilder.HandleDBConfigurationForDepartmentRole();
            modelBuilder.HandleDBConfigurationForVenueCity();
            modelBuilder.HandleDBConfigurationForRole();
            modelBuilder.HandleDBConfigurationForSettings();



        }


        #endregion


    }
}
