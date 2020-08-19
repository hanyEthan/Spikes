using Microsoft.EntityFrameworkCore;
using XCore.Services.Hiring.Core.Constants;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.Core.DataLayer.Context.Configurations
{
    public static class ApplicationsDBConfigurations
    {
        public static void HandleApplicationsDBConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.ApplicationsTable, DBConstants.Schema).HasKey(x => x.Id);

                #endregion
                #region relations.

                entity.HasMany(x => x.Answers)
                      .WithOne(x => x.Application)
                      .HasForeignKey(x => x.ApplicationId);

                entity.HasOne(x => x.Candidate)
                      .WithMany()
                      .HasForeignKey(x => x.CandidateId);

                entity.HasOne(x => x.Advertisement)
                      .WithMany()
                      .HasForeignKey(x => x.AdvertisementId);

                entity.HasOne(x => x.HiringStep)
                      .WithMany()
                      .HasForeignKey(x => x.HiringStepId);

                #endregion
                #region props.

                // common
                entity.Property(x => x.Code).HasColumnName("Code");
                entity.Ignore(x => x.Name);
                entity.Ignore(x => x.NameCultured);
                entity.Property(x => x.IsActive).HasColumnName("IsActive");
                entity.Property(x => x.CreatedDate).HasColumnName("DateCreated");
                entity.Property(x => x.CreatedBy).HasColumnName("CreatedBy");
                entity.Property(x => x.ModifiedDate).HasColumnName("DateModified");
                entity.Property(x => x.ModifiedBy).HasColumnName("ModifiedBy");
                entity.Property(x => x.MetaData).HasColumnName("MetaData");

                // model
                entity.Property(x => x.AppId).HasColumnName("AppId");
                entity.Property(x => x.ModuleId).HasColumnName("ModuleId");

                #endregion
            });
        }
    }
}
