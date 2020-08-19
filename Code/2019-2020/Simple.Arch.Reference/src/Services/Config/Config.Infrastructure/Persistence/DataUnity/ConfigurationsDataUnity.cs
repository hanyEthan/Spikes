using System.Threading;
using System.Threading.Tasks;
using Mcs.Invoicing.Services.Config.Application.Common.Contracts;
using Mcs.Invoicing.Services.Config.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Mcs.Invoicing.Services.Config.Infrastructure.Persistence.DataUnity
{
    public class ConfigurationsDataUnity : IConfigurationDataUnity
    {
        #region props

        public bool? Initialized { get; protected set; }
        private readonly DbContext _DataContext;

        public IModulesRepository Modules { get; private set; }
        public IConfigItemsRepository ConfigItems { get; private set; }

        public IModulesReadOnlyRepository ModulesReadOnly { get; private set; }
        public IConfigItemsReadOnlyRepository ConfigItemsReadOnly { get; private set; }

        #endregion
        #region cst.

        public ConfigurationsDataUnity(ConfigDbContext dataContext, 
                                       IModulesRepository modules,
                                       IModulesReadOnlyRepository modulesReadOnly,
                                       IConfigItemsRepository configItems,
                                       IConfigItemsReadOnlyRepository configItemsReadOnly)
        {
            this._DataContext = dataContext;

            this.Modules = modules;
            this.ModulesReadOnly = modulesReadOnly;
            this.ConfigItems = configItems;
            this.ConfigItemsReadOnly = configItemsReadOnly;

            this.Initialized = Initialize();
        }

        #endregion

        #region publics

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await _DataContext.SaveChangesAsync(cancellationToken);
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this._DataContext != null;
            isValid = isValid && (this.Modules?.Initialized.GetValueOrDefault() ?? false)
                              && (this.ModulesReadOnly?.Initialized.GetValueOrDefault() ?? false)
                              && (this.ConfigItems?.Initialized.GetValueOrDefault() ?? false)
                              && (this.ConfigItemsReadOnly?.Initialized.GetValueOrDefault() ?? false);

            return isValid;
        }

        #endregion
    }
}
