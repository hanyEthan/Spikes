using Microsoft.EntityFrameworkCore;
using XCore.Services.Hiring.Core.Constants;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.Core.DataLayer.Context.Configurations
{
    public static class HiringStepsDBConfigurations
    {
        public static void HandleHiringStepsDBConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HiringStep>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.HiringStepsTable, DBConstants.Schema).HasKey(x => x.Id);

                #endregion
                #region relations.

                entity.HasOne(x => x.HiringProcess)
                      .WithMany(x => x.HiringSteps)
                      .HasForeignKey(x => x.HiringProcessId);

                #endregion
                #region props.
                // ignore

                // common
                entity.Property(x => x.Code).HasColumnName("Code");
                entity.Property(x => x.Name).HasColumnName("Name");
                entity.Property(x => x.NameCultured).HasColumnName("NameCultured");
                entity.Property(x => x.IsActive).HasColumnName("IsActive");
                entity.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                entity.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                entity.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                entity.Property(x => x.MetaData).HasColumnName("MetaData");

                // model

                #endregion
            });
        }
    }
}
