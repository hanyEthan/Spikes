using Mcs.Invoicing.Services.Config.Application.Common.Contracts;
using Mcs.Invoicing.Services.Config.Domain.Entities;
using Mcs.Invoicing.Services.Config.Infrastructure.Persistence.Context;
using Mcs.Invoicing.Services.Core.Framework.Infrastructure.Models.Repositories.Handlers;

namespace Mcs.Invoicing.Services.Config.Infrastructure.Persistence.Repositories
{
    public class ConfigItemsRepository : DbRepository<ConfigItem>, IConfigItemsRepository
    {
        #region props.

        public bool? Initialized { get; protected set; }
        
        #endregion
        #region cst.

        public ConfigItemsRepository(ConfigDbContext context) : base(context)
        {
            this.Initialized = Initialize();
        }

        #endregion
        #region IConfigItemsRepository

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && base.context != null;

            return isValid;
        }

        #endregion
    }
}
