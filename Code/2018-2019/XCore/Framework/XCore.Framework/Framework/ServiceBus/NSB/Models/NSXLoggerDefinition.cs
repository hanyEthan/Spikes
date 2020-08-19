//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using NServiceBus.Logging;

//namespace XCore.Framework.Framework.ServiceBus.Models
//{
//    #region definition

//    class NSXLoggerDefinition : LoggingFactoryDefinition
//    {
//        LogLevel level = LogLevel.Info;

//        public void Level( LogLevel level )
//        {
//            this.level = level;
//        }

//        protected override ILoggerFactory GetLoggingFactory()
//        {
//            return new NSXLoggerFactory( level );
//        }
//    }
//    #endregion
//}
