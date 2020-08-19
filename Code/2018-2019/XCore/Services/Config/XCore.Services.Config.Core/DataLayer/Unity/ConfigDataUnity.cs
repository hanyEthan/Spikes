using System;
using XCore.Services.Config.Core.DataLayer.Context;
using XCore.Services.Config.Core.DataLayer.Contracts;
using XCore.Services.Config.Core.DataLayer.Repositories;

namespace XCore.Services.Config.Core.DataLayer.Unity
{
    public class ConfigDataUnity<TSettings> : IDisposable where TSettings : IConfigDataUnitySettings, new()
    {
        #region props.

        #region Context

        ConfigDataContext context;

        #endregion
        #region App

        private IAppsRepository _Apps;
        public IAppsRepository Apps
        {
            get
            {
                return _Apps = _Apps ?? (_Apps = new AppsRepository(this.context));
            }
        }

        #endregion
        #region Modules

        private IModulesRepository _Modules;
        public IModulesRepository Modules
        {
            get
            {
                return _Modules = _Modules ?? (_Modules = new ModulesRepository(this.context));
            }
        }

        #endregion
        #region Config

        private IConfigsRepository _Configs;
        public IConfigsRepository Configs
        {
            get
            {
                return _Configs = _Configs ?? (_Configs = new ConfigsRepository(this.context));
            }
        }

        #endregion

        #endregion
        #region cst.

        public ConfigDataUnity()
        {
            var settings = new TSettings();
            this.context = new ConfigDataContext(settings.DBConnectionName);
        }

        #endregion
        #region publics

        public void Save()
        {
            context.SaveChanges();
        }

        #endregion
        #region IDisposable

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
