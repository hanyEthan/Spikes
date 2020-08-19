using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Common.Bases.Context
{
    [Serializable]
    public class SystemSecurityContext : XSecurityContext
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
