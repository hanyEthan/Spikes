using Microsoft.EntityFrameworkCore;
using XCore.Services.Hiring.Core.Constants;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.Core.DataLayer.Context.Configurations
{
    public static class QuestionsDBConfigurations
    {
        public static void HandleQuestionsDBConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.QuestionsTable, DBConstants.Schema).HasKey(x => x.Id);

                #endregion
                #region relations.

                entity.HasOne(x => x.Advertisement)
                      .WithMany(x => x.Questions)
                      .HasForeignKey(x => x.AdvertisementId)
                      .OnDelete(DeleteBehavior.Restrict);

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

                #endregion
            });
        }
    }
}
