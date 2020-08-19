using System;

namespace XCore.Framework.Infrastructure.Context.Execution.Support
{
    [Serializable]
    public class SystemSecurityContext : SecurityContextBase
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
