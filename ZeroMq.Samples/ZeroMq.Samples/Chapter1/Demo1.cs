using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Konsole;
using Konsole.Drawing;
using Konsole.Layouts;
using NetMQ.Sockets;

namespace ZeroMq.Samples.Chapter1
{


    public class Demo1
    {
        public static void Run()
        {
            var keyboard = new Keyboard();
            Console.Clear();
            var w = new Window(Console.WindowWidth, Console.WindowHeight / 2);
            var client = w.SplitLeft("client");
            var server = w.SplitRight("server");

            var t1 = Task.Run(() => RunClient(client, keyboard));
            var t2 = Task.Run(() => RunServer(server, keyboard));

            Console.WriteLine("press 'q' to quit.");
            keyboard.WaitForKeyPress('q');
            Console.WriteLine("waiting for server and client to stop");
            Task.WaitAll(t1, t2);
            Console.WriteLine("All stopped, press any key to quit.");
            Console.ReadKey();
        }

        public static void RunServer(IConsole console, IKeyboard keyboard)
        {
            bool stopServer = false;
            keyboard.OnCharPressed('s', c => ServerAction(c, console));
            keyboard.OnCharPressed('q', c => { stopServer = true; });

            console.WriteLine(ConsoleColor.Yellow, "starting server");
            
            //using (var server = new ResponseSocket("tcp://*:1234"))
            //{
            //    con.WriteLine(ConsoleColor.Yellow, "ready, press '1' for server action, and 'x' to shutdown.");
            //    k.OnCharPress(c => ServerAction(c, con), 'c');
            //    k.OnCharPress(c => { shutdown = true; }, 'x');

            //    while(!shutdown) Thread.Sleep(50);
            //}

            console.WriteLine(ConsoleColor.Yellow, "ready, press 's' for server action.");

            while (! stopServer)
            {
                Thread.Sleep(50);
            }
            console.WriteLine(ConsoleColor.Yellow, "server stopped.");
        }

        private static void ServerAction(char c, IConsole con)
        {
            con.WriteLine($"Got it! You presses {c}");
        }


        
        public static void RunClient(IConsole con, IKeyboard k)
        {
            bool stopClient = false;
            k.OnCharPressed('c', c=> ClientAction(c, con));
            k.OnCharPressed('q', c => { stopClient = true; });
            con.WriteLine(ConsoleColor.Green, "client starting");
            con.WriteLine(ConsoleColor.Green, "ready, press 'c' for client action.");
            while (!stopClient)
            {
                Thread.Sleep(50);
            }
            con.WriteLine(ConsoleColor.Green, "client stopped");
        }

        private static void ClientAction(char c, IConsole con)
        {
            con.WriteLine($"Got it! You presses {c}");
        }


    }
}
