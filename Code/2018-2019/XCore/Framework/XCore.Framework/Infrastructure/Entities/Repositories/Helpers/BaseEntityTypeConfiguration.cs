using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace XCore.Framework.Infrastructure.Entities.Repositories.Helpers
{
    public abstract class BaseEntityTypeFluentMapperConfiguration<T> : IEntityTypeConfiguration<T> where T : class
    {
        #region props.

        protected string TableName { get; set; }

        #endregion
        #region cst.

        public BaseEntityTypeFluentMapperConfiguration(string tableName)
        {
            this.TableName = tableName;
        }

        #endregion

        #region IEntityTypeConfiguration

        public abstract void Configure(EntityTypeBuilder<T> builder);

        #endregion
    }
}
