using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCore.Utilities.Logger.Framework.Logging.Contracts
{
    public interface ILogQueueManager
    {
        bool initialized { get; }
        void startListening();
        void stopListening();
        void AddSubscriber( ILogQueueSubscriber subscriber );
        void AddSubscribers( List<ILogQueueSubscriber> subscribers );
    }
}
