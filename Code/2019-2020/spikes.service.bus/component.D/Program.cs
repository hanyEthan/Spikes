using System;
using component.D.Consumers;
using Core.Framework;
using MassTransit;

namespace component.D
{
    class Program
    {
        static void Main(string[] args)
        {
            var SB = new ServiceBus("Component.D.Queue", e =>
            {
                e.Consumer<MessageConsumers>();
            });

            SB.Start().GetAwaiter().GetResult();
            Console.ReadKey();
            SB.Stop().GetAwaiter().GetResult();
        }
    }
}
