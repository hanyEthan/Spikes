using Microsoft.EntityFrameworkCore;
using XCore.Services.Attachments.Core.Models;

namespace XCore.Services.Attachments.Core.DataLayer.Context
{
    public class AttachmentDataContext : DbContext
    {
       
        #region DbSets

        public virtual DbSet<Attachment> Attachments { get; set; }

        #endregion

        #region cst.

        public AttachmentDataContext(DbContextOptions<AttachmentDataContext> options) : base(options)
        {
        }

        #endregion
        #region base

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HandleDBConfigurationForAttachment();
        }
        #endregion
    }
}
