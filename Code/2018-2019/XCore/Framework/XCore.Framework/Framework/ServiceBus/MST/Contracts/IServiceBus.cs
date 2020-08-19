using System;
using System.Threading.Tasks;
using MassTransit;
using XCore.Framework.Framework.ServiceBus.MST.Support;

namespace XCore.Framework.Framework.ServiceBus.Contracts
{
    public interface IServiceBus
    {
        #region props.

        bool Initialized { get; }
        ServiceBusConfiguration Configurations { get; }

        #endregion
        #region publics.

        Task Start();
        Task Stop();

        Task Send<TContract, TMessage>(TMessage message, string endpoint) where TMessage : class, TContract where TContract : class;
        Task Publish<TContract, TMessage>(TMessage message) where TMessage : class, TContract where TContract : class;

        void Subscribe<TConsumer, TMessage>() where TConsumer : class, MassTransit.IConsumer<TMessage>, new() where TMessage : class;

        IBusControl GetBusControl(IServiceProvider serviceProvider);

        #endregion
    }
}
