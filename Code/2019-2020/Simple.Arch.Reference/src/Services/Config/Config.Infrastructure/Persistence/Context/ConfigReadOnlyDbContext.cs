using Microsoft.EntityFrameworkCore;

namespace Mcs.Invoicing.Services.Config.Infrastructure.Persistence.Context
{
    public class ConfigReadOnlyDbContext : ConfigDbContext
    {
        #region cst.

        public ConfigReadOnlyDbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion
    }
}
