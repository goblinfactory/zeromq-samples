using System;
using Konsole.Menus;

namespace ZeroMq.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            var menu = new Menu("Zeromq demos", ConsoleKey.Escape, 60,

                new MenuItem('1', "Chapter 1) send and recieve strings", Chapter1.Program.Run)
            
            );
            menu.OnBeforeMenuItem += m => Console.Clear();
            menu.OnAfterMenuItem += m=> { Console.Clear(); menu.Refresh(); };
            menu.Run();

        }

    }
}

