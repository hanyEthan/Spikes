using Microsoft.EntityFrameworkCore;
using XCore.Services.Docs.Core.Models;

namespace XCore.Services.Docs.Core.DataLayer.Context
{
    public class DocumentDataContext : DbContext
    {
        #region DbSets

        public virtual DbSet<Document> Document { get; set; }

        #endregion

        #region cst.

        public DocumentDataContext(DbContextOptions<DocumentDataContext> options) : base(options)
        {
        }

        #endregion
        #region base

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HandleDBConfigurationForDocument();
        }

        #endregion
    }
}
