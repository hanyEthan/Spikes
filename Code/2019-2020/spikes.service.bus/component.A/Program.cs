using System;
using component.A.Messages;
using component.A.Models;
using Core.Framework;

namespace component.A
{
    class Program
    {
        static void Main()
        {
            var SB = new ServiceBus(null);

            while (true)
            {
                Console.WriteLine("Press Enter to Publish [A].");
                Console.ReadLine();
                SB.Publish<IMessageA, MessageA>(new MessageA() { Value = "AA" , V2="VV" }).GetAwaiter().GetResult();
            }
        }
    }
}
