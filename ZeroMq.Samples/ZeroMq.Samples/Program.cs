using System;
using Konsole;
using Konsole.Drawing;
using Konsole.Menus;
using ZeroMq.Samples.Chapter1;


namespace ZeroMq.Samples
{
    partial class Program
    {

        static void Main(string[] args)
        {

            var menu = new Menu("Zeromq demos", ConsoleKey.Escape, 60,

                new MenuItem('1', "send and recieve strings", Demo1.Run)
            
            );
            menu.OnBeforeMenuItem += m => Console.Clear();
            menu.OnAfterMenuItem += m=> { Console.Clear(); menu.Refresh(); };
            menu.Run();

        }
    }
}

