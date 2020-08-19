using System;
using System.Threading.Tasks;
using XCore.Services.Configurations.Core.DataLayer.Context;
using XCore.Services.Configurations.Core.DataLayer.Contracts;

namespace XCore.Services.Configurations.Core.DataLayer.Unity
{
    public class ConfigurationDataUnity : IConfigurationDataUnity
    {
        #region props

        public bool? Initialized { get; protected set; }
        private readonly ConfigDataContext _DataContext;

        
        public IModulesRepository Modules { get; private set; }

        public IConfigsRepository Configs { get; private set; }

        public IAppsRepository Apps { get; private set; }

        #endregion
        #region cst.

        public ConfigurationDataUnity(ConfigDataContext dataContext, IModulesRepository modules, IConfigsRepository configs, IAppsRepository apps)
        {
            _DataContext = dataContext;

            Modules = modules;
            Configs = configs;
            Apps = apps;

            this.Initialized = Initialize();
        }

        #endregion

        #region publics

        public async Task SaveAsync()
        {
            await _DataContext.SaveChangesAsync();
        }

        #endregion
        #region helpers.

        private bool Initialize()
        {
            bool isValid = true;

            isValid = isValid && this._DataContext != null;
            isValid = isValid && (this.Apps?.Initialized.GetValueOrDefault() ?? false)
                 && (this.Modules?.Initialized.GetValueOrDefault() ?? false)
                && (this.Configs?.Initialized.GetValueOrDefault() ?? false);



            return isValid;
        }

        #endregion
    }
}
