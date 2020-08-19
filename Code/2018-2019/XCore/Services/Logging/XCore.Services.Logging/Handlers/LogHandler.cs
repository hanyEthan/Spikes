using System.Threading.Tasks;
using XCore.Framework.Utilities;

namespace XCore.Services.Logging.Handlers
{
    public class LogHandler : MassTransit.IConsumer<XLogger.ILogMessage>
    {
        #region MassTransit.IConsumer<ILogMessage>

        public async Task Consume(MassTransit.ConsumeContext<XLogger.ILogMessage> context)
        {
            XLogger.Relay(context.Message.Message);
        }

        #endregion
    }
}
