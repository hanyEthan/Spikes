using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using component.A.Messages;
using component.B.Messages;
using MassTransit;

namespace component.C.Consumers
{
    public class MessageConsumers : IConsumer<IMessageA>, 
                                    IConsumer<IMessageB>
    {
        #region MassTransit.IConsumer<IMessageA>

        public async Task Consume(ConsumeContext<IMessageA> context)
        {
            Log(context.Message);
        }

        #endregion
        #region MassTransit.IConsumer<IMessageB>

        public async Task Consume(ConsumeContext<IMessageB> context)
        {
            Log(context.Message);
        }

        #endregion

        #region helpers.

        private void Log(IMessageA message)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.WriteLine($"message Received : {message.Value}");

            Console.ResetColor();
        }
        private void Log(IMessageB message)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"message Received : {message.Value}");

            Console.ResetColor();
        }

        #endregion
    }
}
