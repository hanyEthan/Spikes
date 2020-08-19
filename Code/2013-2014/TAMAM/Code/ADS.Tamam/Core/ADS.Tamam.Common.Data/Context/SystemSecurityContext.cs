using System;

namespace ADS.Tamam.Common.Data.Context
{
    [Serializable]
    public class SystemSecurityContext : SecurityContext
    {
        #region props ...

        private static object syncRoot = new Object();

        private static volatile SystemSecurityContext instance;
        public static SystemSecurityContext Instance
        {
            get
            {
                if ( instance == null )
                {
                    lock ( syncRoot )
                    {
                        if ( instance == null ) instance = new SystemSecurityContext();
                    }
                }

                return instance;
            }
        }

        #endregion
        #region cst ...

        private SystemSecurityContext() { }
        
        #endregion
    }
}
