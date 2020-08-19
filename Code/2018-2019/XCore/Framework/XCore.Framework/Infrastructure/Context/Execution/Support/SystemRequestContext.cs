using System;

namespace XCore.Framework.Infrastructure.Context.Execution.Support
{
    [Serializable]
    public class SystemRequestContext : RequestContext
    {
        #region props ...

        private static object syncRoot = new Object();

        private static volatile SystemRequestContext instance;
        public static SystemRequestContext Instance
        {
            get
            {
                if ( instance == null )
                {
                    lock ( syncRoot )
                    {
                        if ( instance == null ) instance = new SystemRequestContext();
                    }
                }

                return instance;
            }
        }

        #endregion
        #region cst ...

        private SystemRequestContext() { }

        #endregion
    }
}
