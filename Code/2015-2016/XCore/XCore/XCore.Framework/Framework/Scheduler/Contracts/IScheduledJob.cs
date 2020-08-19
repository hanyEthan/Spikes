using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCore.Framework.Framework.Scheduler.Contracts
{
    public interface IScheduledJob
    {
        bool Initialized { get; }
        Task Execute();
    }
}
