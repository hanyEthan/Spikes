using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using XCore.Services.Lookups.Core.Models.Domain;
using XCore.Services.Lookups.Core.DataLayer.Context.Configurations;

namespace XCore.Services.Lookups.Core.DataLayer.Context
{
    public class LookupsDataContext : DbContext
    {
        #region Props.

        #endregion
        #region DbSets

        public virtual DbSet<LookupCategory> LookupCategories { get; set; }

        #endregion

        #region cst.

        public LookupsDataContext(DbContextOptions<LookupsDataContext> options) : base(options)
        {
        }

        #endregion
        #region base

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HandleDBConfigurationForLookupCategories();
            modelBuilder.HandleDBConfigurationForLookups();
        }

        #endregion
    }
}
