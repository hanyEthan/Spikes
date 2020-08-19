using System;
using System.Collections.Generic;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Support
{
    public class MessageConsumersConfigurator
    {
        #region nested.

        public class MessagesConsumerDefinition
        {
            public Type ConsumerType { get; set; }
            public string QueueName { get; set; }
            public ushort? MaxConcurrentMessages { get; set; }
            public ushort? RetryCount { get; set; }
            public long? RetryIntervalInMilliseconds { get; set; }

            public int? CircuitBreakerTrackingPeriodInSeconds { get; set; }
            public ushort? CircuitBreakerTripThreshold { get; set; }
            public ushort? CircuitBreakerActiveThreshold { get; set; }
            public int? CircuitBreakerResetIntervalInSeconds { get; set; }
        }

        #endregion
        #region props.

        public List<MessagesConsumerDefinition> ConsumerDefinitions { get; private set; }

        #endregion
        #region cst.

        public MessageConsumersConfigurator()
        {
            this.ConsumerDefinitions = new List<MessagesConsumerDefinition>();
        }

        #endregion
        #region publics.

        public void AddAsyncConsumer<TMessageConsumer>(string queueName, 
                                                       ushort? maxConcurrentMessages = null, 
                                                       ushort? retryCount = null, 
                                                       long? retryIntervalInMilliseconds = null,
                                                       int? circuitBreakerTrackingPeriodInSeconds = null,
                                                       ushort? circuitBreakerTripThreshold = null,
                                                       ushort? circuitBreakerActiveThreshold = null,
                                                       int? circuitBreakerResetIntervalInSeconds = null)
        {
            this.ConsumerDefinitions.Add(new MessagesConsumerDefinition()
            {
                ConsumerType = typeof(TMessageConsumer),
                QueueName = queueName,
                MaxConcurrentMessages = maxConcurrentMessages,
                RetryCount = retryCount,
                RetryIntervalInMilliseconds = retryIntervalInMilliseconds,
                CircuitBreakerTrackingPeriodInSeconds = circuitBreakerTrackingPeriodInSeconds,
                CircuitBreakerTripThreshold = circuitBreakerTripThreshold,
                CircuitBreakerActiveThreshold = circuitBreakerActiveThreshold,
                CircuitBreakerResetIntervalInSeconds = circuitBreakerResetIntervalInSeconds,
            });
        }

        #endregion
    }
}
