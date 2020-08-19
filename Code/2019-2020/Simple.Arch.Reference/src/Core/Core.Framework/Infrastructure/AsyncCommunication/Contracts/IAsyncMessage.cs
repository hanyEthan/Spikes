namespace Mcs.Invoicing.Core.Framework.Infrastructure.AsyncCommunication.Contracts
{
    public interface IAsyncMessage<TMessage> : MassTransit.IConsumer<TMessage> where TMessage : class
    {
    }
}
