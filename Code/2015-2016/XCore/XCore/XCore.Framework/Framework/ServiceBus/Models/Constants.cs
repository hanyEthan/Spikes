namespace XCore.Framework.Framework.ServiceBus.Models
{
    public static class Constants
    {
        public static class NSB
        {
            public const string InstanceName = "Services.NSB.Endpoint.Instance.Name";
            public const string Queues_Maintain = "Services.NSB.Endpoint.Queues.Maintain";
            public const string Connection_Transport = "Services.NSB.ConnectionStrings.Transport";
            public const string Connection_Persistance = "Services.NSB.ConnectionStrings.Persistance";

            public const string Failures_Immediate_NumberOfRetries = "Services.NSB.Endpoint.Queues.Retry.Immediate.Number";
            public const string Failures_Delayed_NumberOfRetries = "Services.NSB.Endpoint.Queues.Retry.Delayed.Number";
            public const string Failures_Delayed_TimeIncreases = "Services.NSB.Endpoint.Queues.Retry.Delayed.TimeIncreases";
            public const string ConcurrentThreads = "Services.NSB.Endpoint.Threads.Concurrent";
        }
    }
}
