using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Framework.Utilities;
using Microsoft.EntityFrameworkCore;
using XCore.Framework.Infrastructure.Messaging.Queues.Models;

namespace XCore.Framework.Infrastructure.Messaging.Queues.Repositories.Context
{
    public class QueueDataContext : DbContext
    {
        #region Props.

        public string TableName { get; set; }

        protected const string DefaultTableName = "MQMessages";
        protected const string DefaultConnectionString = "XCore.DB";

        #endregion
        #region DbSets

        public virtual DbSet<MQMessage> MQMessages { get; set; }

        #endregion

        #region cst.

        public QueueDataContext() : this(DefaultConnectionString, null)
        {
        }
        public QueueDataContext(string connectionString) : this(new DbContextOptionsBuilder<QueueDataContext>().UseSqlServer(XDB.GetConnectionString(connectionString)).Options, null)
        {
        }
        public QueueDataContext(string connectionString, string tableName) : this(new DbContextOptionsBuilder<QueueDataContext>().UseSqlServer(XDB.GetConnectionString(connectionString)).Options, tableName)
        {
        }
        public QueueDataContext(DbContextOptions options) : this(options, null)
        {
        }
        public QueueDataContext(DbContextOptions options, string tableName) : base(options)
        {
            //HandleReferences();
            this.TableName = tableName ?? DefaultTableName;
        }

        #endregion

        #region base

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new QueueDataContextConfigurations.MQMessageConfiguration(this.TableName));
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
