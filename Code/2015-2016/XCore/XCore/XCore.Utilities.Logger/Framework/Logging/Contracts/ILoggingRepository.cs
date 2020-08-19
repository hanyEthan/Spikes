using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Logger.Framework.Logging.Models;

namespace XCore.Utilities.Logger.Framework.Logging.Contracts
{
    public interface ILoggingRepository
    {
        bool AddLogs( List<LogMessage> logs );
    }
}
