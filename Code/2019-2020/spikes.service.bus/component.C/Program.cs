using System;
using component.C.Consumers;
using Core.Framework;
using MassTransit;

namespace component.C
{
    class Program
    {
        static void Main()
        {
            var SB = new ServiceBus("Component.C.Queue", e =>
            {
                e.Consumer<MessageConsumers>();
            });

            SB.Start().GetAwaiter().GetResult();
            Console.ReadKey();
            SB.Stop().GetAwaiter().GetResult();
        }
    }
}
