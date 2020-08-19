using Microsoft.EntityFrameworkCore;
using XCore.Services.Hiring.Core.Constants;
using XCore.Services.Hiring.Core.Models.Domain;

namespace XCore.Services.Hiring.Core.DataLayer.Context.Configurations
{
    public static class ApplicationHistoryDBConfigurations
    {
        public static void HandleApplicationHistoryDBConfigurations(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationHistory>(entity =>
            {
                #region table.

                entity.ToTable(DBConstants.ApplicationHistoryTable, DBConstants.Schema).HasKey(x => x.Id);

                #endregion
                #region relations.               

                #endregion
                #region props.

                // ignore
                entity.Ignore(x => x.Name);
                entity.Ignore(x => x.NameCultured);
                entity.Ignore(x => x.Code);
                entity.Ignore(x => x.CreatedBy);
                entity.Ignore(x => x.IsActive);
                entity.Ignore(x => x.ModifiedDate);
                entity.Ignore(x => x.ModifiedBy);
                entity.Ignore(x => x.MetaData);

                // common
                entity.Property(x => x.ActionId).HasColumnName("ActionId");
                entity.Property(x => x.CreatedDate).HasColumnName("CreatedDate");
                entity.Property(x => x.Mode).HasColumnName("Mode");
                entity.Property(x => x.ModelId).HasColumnName("ModelId");
                entity.Property(x => x.StatusNew).HasColumnName("StatusNew");
                entity.Property(x => x.StatusOld).HasColumnName("StatusOld");
                entity.Property(x => x.UserId).HasColumnName("UserId");
                entity.Property(x => x.UserName).HasColumnName("UserName");


                #endregion
            });
        }
    }
}
