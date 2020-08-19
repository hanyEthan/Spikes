using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XCore.Utilities.Extensions.CMD;

namespace XCore.Framework.Utilities
{
    #region XConsole

    public static class XConsole
    {
        public static void Run( Action<XConsoleSetup> setup )
        {
            var ls = new XConsoleSetup()
                .DefineAction( ConsoleKey.X , "Exit." , () =>
                {
                    return XConsoleSetup.Engine.Terminate;
                } );

            setup( ls );

            ls.Run();

        }
    }

    #endregion
    #region XConsoleSetup

    public class XConsoleSetup
    {
        public enum Engine
        {
            Terminate,
            Continue
        }

        class ActionDescriptor
        {
            public Func<Engine> Action { get; set; }
            public string Description { get; set; }
        }
        Dictionary<ConsoleKey , ActionDescriptor> actions = new Dictionary<ConsoleKey , ActionDescriptor>();

        internal XConsoleSetup() { }

        public XConsoleSetup DefineAction( ConsoleKey key , String description , Func<Engine> action )
        {
            this.actions[key] = new ActionDescriptor()
            {
                Action = action ,
                Description = description
            };

            return this;
        }

        public XConsoleSetup DefineAction( ConsoleKey key , String description , Action action )
        {
            this.actions[key] = new ActionDescriptor()
            {
                Action = () =>
                {
                    action();
                    return Engine.Continue;
                } ,
                Description = description
            };

            return this;
        }

        internal void Run()
        {
            Console.WriteLine();
            Console.WriteLine();

            using ( ConsoleColor.Green.AsForegroundColor() )
            {
                Console.WriteLine( "Help:" );
                foreach ( var kvp in this.actions )
                {
                    Console.WriteLine( "\t" + kvp.Key + " -> " + kvp.Value.Description );
                }
            }
            var response = Console.ReadKey().Key;

            if ( this.actions.Keys.Any( vr => vr == response ) )
            {
                Console.WriteLine();
                Console.WriteLine();

                var next = this.actions[response].Action();
                if ( next == Engine.Continue )
                {
                    this.Run();
                }
            }
            else
            {
                Console.WriteLine( "unknown..." );
                this.Run();
            }
        }
    }

    #endregion
}
