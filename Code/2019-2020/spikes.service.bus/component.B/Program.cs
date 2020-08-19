using System;
using component.B.Messages;
using component.B.Models;
using Core.Framework;

namespace component.B
{
    class Program
    {
        static void Main()
        {
            var SB = new ServiceBus(null);

            while (true)
            {
                Console.WriteLine("Press Enter to Publish [B].");
                Console.ReadLine();
                SB.Publish<IMessageB, MessageB>(new MessageB() { Value = "BB" }).GetAwaiter().GetResult();
            }
        }
    }
}
