using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Common.Bases.Context
{
    [Serializable]
    public class SystemRequestContext : XRequestContext
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
