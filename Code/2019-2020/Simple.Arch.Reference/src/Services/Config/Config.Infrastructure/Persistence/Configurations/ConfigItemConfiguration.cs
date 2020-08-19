using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Config.Infrastructure.Persistence.Support;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mcs.Invoicing.Services.Config.Infrastructure.Persistence.Configurations
{
    public class ConfigItemConfiguration : IEntityTypeConfiguration<ConfigItem>
    {
        public void Configure(EntityTypeBuilder<ConfigItem> builder)
        {
            #region table.

            builder.ToTable(PersistenceConstants.Table_Configs, PersistenceConstants.DB_Schema)
                   .HasKey(x => x.Id);

            #endregion
            #region props.

            // model
            builder.Property(x => x.Key).HasColumnName("Key").IsRequired();
            builder.Property(x => x.Value).HasColumnName("Value");
            builder.Property(x => x.Description).HasColumnName("Description");

            // Base Entity
            builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
            builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").IsRequired();
            builder.Property(x => x.CreationDateTimeUtc).HasColumnName("CreationDateTimeUtc");
            builder.Property(x => x.LastModificationDateTimeUtc).HasColumnName("LastModificationDateTimeUtc");
            builder.Property(x => x.CreatedByUserId).HasColumnName("CreatedByUserId");
            builder.Property(x => x.LastModifiedByUserId).HasColumnName("LastModifiedByUserId");
            builder.Property(x => x.DeletionDateTimeUtc).HasColumnName("DeletionDateTimeUtc");
            builder.Property(x => x.DeletedByUserId).HasColumnName("DeletedByUserId");

            #endregion
            #region relations.

            // one-many : Module.
            builder.HasOne(config => config.Module)
                   .WithMany(module => module.Configurations)
                   .HasForeignKey(config => config.ModuleId);

            #endregion
        }
    }
}
