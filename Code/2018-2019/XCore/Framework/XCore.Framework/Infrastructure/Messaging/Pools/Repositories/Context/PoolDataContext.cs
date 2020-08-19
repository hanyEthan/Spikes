using Microsoft.EntityFrameworkCore;
using XCore.Framework.Infrastructure.Messaging.Pools.Models;
using XCore.Framework.Utilities;

namespace XCore.Framework.Infrastructure.Messaging.Pools.Repositories.Context
{
    public class PoolDataContext : DbContext
    {
        #region props.

        public string TableName { get; set; }

        protected const string DefaultTableName = "MessagesPool";
        protected const string DefaultConnectionString = "XCore.DB";

        #endregion
        #region DbSets

        public virtual DbSet<PoolMessage> PoolMessages { get; set; }

        #endregion

        #region cst.

        public PoolDataContext() : this(DefaultConnectionString, null )
        {
        }
        public PoolDataContext(string connectionString) : this(new DbContextOptionsBuilder<PoolDataContext>().UseSqlServer(XDB.GetConnectionString(connectionString)).Options, null)
        {
        }
        public PoolDataContext(string connectionString , string tableName) : this(new DbContextOptionsBuilder<PoolDataContext>().UseSqlServer(XDB.GetConnectionString(connectionString)).Options, tableName)
        {
        }
        public PoolDataContext(DbContextOptions options) : this(options, null)
        {
        }
        public PoolDataContext(DbContextOptions options, string tableName) : base(options)
        {
            //HandleReferences();
            this.TableName = tableName ?? DefaultTableName;
        }

        #endregion
        #region base

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PoolDataContextConfigurations.PoolMessageConfiguration(this.TableName));
        }

        #endregion
        #region helpers.

        //private void HandleReferences()
        //{
        //    var x = typeof( SqlProviderServices );
        //    var y = x.ToString();
        //}

        #endregion
    }
}
