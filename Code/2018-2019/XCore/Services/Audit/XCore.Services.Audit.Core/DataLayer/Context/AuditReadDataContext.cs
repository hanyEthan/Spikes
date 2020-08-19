using Microsoft.EntityFrameworkCore;
using XCore.Services.Audit.Core.DataLayer.Context.Configurations;

namespace XCore.Services.Audit.Core.DataLayer.Context
{
    public class AuditReadDataContext : AuditDataContext
    {
        #region Props.

        #endregion
        #region DbSets

        #endregion

        #region cst.
        public AuditReadDataContext(DbContextOptions<AuditReadDataContext> options) : base(options)
        {
        }

        #endregion
        #region base

        #endregion
    }
}
