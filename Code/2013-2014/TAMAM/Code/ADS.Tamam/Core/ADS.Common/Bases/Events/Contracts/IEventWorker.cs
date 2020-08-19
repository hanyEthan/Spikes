using System;
using ADS.Common.Bases.Events.Models;
using ADS.Common.Contracts;

namespace ADS.Common.Bases.Events.Contracts
{
    public interface IEventWorker : IBaseHandler
    {
        Guid Id { get; set; }
        EventsWorkerStatus Status { get; }

        bool Start();
        bool Stop();
    }
}
