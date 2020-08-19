using ADS.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADS.Tamam.Modules.Integration.Helpers
{
    public class LogHelper
    {
        public static string BuildSkippedMessage(ILoggable loggable, string reason = "no changes")
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("-------------------------------------------------------------");
            messageBuilder.AppendLine("[" + loggable.Reference + "] Skipped due to " + reason + " " + loggable.GetType());
            messageBuilder.AppendLine("Integration data " + loggable.GetLoggingData());
            messageBuilder.AppendLine("-------------------------------------------------------------");   
            return messageBuilder.ToString();
        }

        public static string BuildMessage(string message, params object[] args)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("-------------------------------------------------------------");
            messageBuilder.AppendLine(string.Format(message, args));
            messageBuilder.AppendLine("-------------------------------------------------------------");
            return messageBuilder.ToString();
        }      
    }
}
