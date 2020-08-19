using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Config.Infrastructure.Persistence.Support;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mcs.Invoicing.Services.Config.Infrastructure.Persistence.Configurations
{
    public class ModuleConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            #region table.

            builder.ToTable(PersistenceConstants.Table_Modules, PersistenceConstants.DB_Schema)
                   .HasKey(x => x.Id);

            #endregion
            #region props.

            // model
            builder.Property(x => x.Description).HasColumnName("Description");

            // Base Entity
            builder.Property(x => x.Code).HasColumnName("Code").IsRequired();
            builder.Property(x => x.Name).HasColumnName("Name").IsRequired();
            builder.Property(x => x.NameCultured).HasColumnName("NameCultured");
            builder.Property(x => x.IsDeleted).HasColumnName("IsDeleted").IsRequired();
            builder.Property(x => x.CreationDateTimeUtc).HasColumnName("CreationDateTimeUtc");
            builder.Property(x => x.LastModificationDateTimeUtc).HasColumnName("LastModificationDateTimeUtc");
            builder.Property(x => x.CreatedByUserId).HasColumnName("CreatedByUserId");
            builder.Property(x => x.LastModifiedByUserId).HasColumnName("LastModifiedByUserId");
            builder.Property(x => x.DeletionDateTimeUtc).HasColumnName("DeletionDateTimeUtc");
            builder.Property(x => x.DeletedByUserId).HasColumnName("DeletedByUserId");

            #endregion
            #region relations.

            #endregion
        }
    }
}
