using System;
using System.Collections.Generic;
using System.Text;

namespace XCore.Services.Configurations.Core.Models
{
    public static class Constants
    {
        #region DB.

        public static class DB
        {
            public const string Schema = "Config";
            public const string TableApps = "Apps";
            public const string TableModules = "Modules";
            public const string TableConfigs = "Configs";
        }

        #endregion

        public static string DateFormatt1="dd/MM/yyyy";
    }
}
