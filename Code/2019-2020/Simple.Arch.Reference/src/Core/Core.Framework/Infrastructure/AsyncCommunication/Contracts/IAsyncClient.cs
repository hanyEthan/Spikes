using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;

namespace Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Contracts
{
    public interface IAsyncClient
    {
        #region props.

        bool Initialized { get; }

        #endregion
        #region publics.

        Task Send<TContract>(TContract message, string endpoint, CancellationToken cancellationToken = default) where TContract : class;
        Task Send<TContract>(TContract message, string endpoint, Action<SendContext<TContract>> context, CancellationToken cancellationToken = default) where TContract : class;

        Task Publish<TContract>(TContract message, CancellationToken cancellationToken = default) where TContract : class;
        Task Publish<TContract>(TContract message, Action<PublishContext<TContract>> context, CancellationToken cancellationToken = default) where TContract : class;

        #endregion
    }
}
