using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Logger.Framework.Logging.Contracts;
using XCore.Utilities.Logger.Framework.Logging.Data.Context;
using XCore.Utilities.Logger.Framework.Logging.Models;

namespace XCore.Utilities.Logger.Framework.Logging.Handlers
{
    public class LoggingHandler : ILoggingHandler
    {
        #region props.

        public bool Initialized { get; private set; }
        public string Name { get { return "LoggingHandler"; } }

        #endregion
        #region cst.

        public LoggingHandler()
        {
            Initialized = true;
        }

        #endregion
        #region ILoggingHandler

        public bool AddLogs( List<LogMessage> logs )
        {
            #region Logic

            using ( var dataLayer = new LoggingRepository( new LoggingDataContext() ) )
            {
                dataLayer.AddLogs( logs );
                dataLayer.Save();
            }

            #endregion
            #region ...

            return true;

            #endregion
        }

        #endregion
    }
}
