using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konsole;
using Konsole.Layouts;
using ZeroMq.Samples.Chapter1;

namespace ZeroMq.Samples
{
    public class Demo
    {
        private readonly string _actionTitle;
        private readonly Action<IConsole, IKeyboard> _startServer;
        private readonly Action<IConsole, IKeyboard> _startClient;
        private readonly char _quitKey;
        public IConsole ClientCon { get; set; }
        public IConsole ServerCon { get; set; }

        public Demo(string actionTitle,  Action<IConsole,IKeyboard> startServer, Action<IConsole, IKeyboard> startClient, char quitKey = 'q')
        {
            _startClient = startClient;
            _quitKey = quitKey;
            _startServer = startServer;
            _actionTitle = actionTitle;
        }

        public void Run()
        {
            var keyboard = new Keyboard();
            Console.Clear();
            var w = new Window(Console.WindowWidth, Console.WindowHeight / 3 * 2);
            ClientCon = w.SplitLeft("client");
            ServerCon = w.SplitRight("server");

            var t1 = Task.Run(() =>  _startClient(ClientCon,keyboard));
            var t2 = Task.Run(() => _startServer(ServerCon, keyboard));

            Console.WriteLine($"1. {_actionTitle}");
            Console.WriteLine("q. to quit.");

            keyboard.WaitForKeyPress(_quitKey);
            Console.WriteLine("waiting for server and client to stop");
            Task.WaitAll(t1, t2);

            Console.WriteLine("All stopped, press any key to quit.");
            Console.ReadKey(true);
        }

    }
}