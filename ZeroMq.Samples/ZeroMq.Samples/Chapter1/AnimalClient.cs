using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Konsole;
using NetMQ.Sockets;

namespace ZeroMq.Samples.Chapter1
{
    public class AnimalClient
    {
        private int _currentDogs = 10;
        private int _currentCats = 15;

        private enum Animal {  Dog, Cat }
        private Animal _processing = Animal.Dog;
        private readonly IConsole _con;
        private readonly IKeyboard _k;

        public AnimalClient(IConsole con, IKeyboard k) 
        {
            _con = con;
            _k = k;
        }

        public void Run()
        {
            _con.WriteLine("'c':set cats 'd':set dogs 'a':process animal 'l':list stock");
            var client = new RequestSocketMicroService(
                "localhost", 1234, _con, _k, 'q', null, null, 
                    new KeyHandler<RequestSocket>('c', SetCats),
                    new KeyHandler<RequestSocket>('d', SetDogs ),
                    new KeyHandler<RequestSocket>('l', ListAnimals),
                    new KeyHandler<RequestSocket>('f', FIN),
                    new KeyHandler<RequestSocket>( 'a', ProcessAnimal)
            );
            client.Run();
        }

        // these methods will be invoked sequentially and on a single thread
        // -----------------------------------------------------------------

        private void ProcessAnimal (IConsole con, RequestSocket socket)
        {
            var catEcho = new ResponseEcho(con, socket, ConsoleColor.Magenta, ConsoleColor.DarkMagenta);
            var dogEcho = new ResponseEcho(con, socket, ConsoleColor.Cyan, ConsoleColor.DarkCyan);

            switch (_processing)
            {
                case Animal.Dog:
                    if (_currentDogs > 0)
                    {
                        _currentDogs--;
                        dogEcho.Request($"process dog #{_currentDogs}");
                    }
                    break;
                case Animal.Cat:
                    if (_currentCats > 0)
                    {
                        _currentCats--;
                        catEcho.Request($"process cat #{_currentDogs}");
                    }
                    break;
            }
        }

        private void SetCats(IConsole con, RequestSocket socket) 
        {
            _processing = Animal.Cat;
            con.WriteLine(ConsoleColor.Green, "now processing cats.");
        }

        private void FIN(IConsole con, RequestSocket socket)
        {
            new ResponseEcho(con, socket, ConsoleColor.Magenta, ConsoleColor.DarkMagenta).Request("[FIN]");
        }

        private void  SetDogs(IConsole con, RequestSocket socket) 
        {
            _processing = Animal.Dog;
            con.WriteLine(ConsoleColor.DarkYellow, "now processing dogs.");
        }

        private void ListAnimals(IConsole con, RequestSocket socket)
        {
            con.WriteLine(ConsoleColor.Green, $"{_currentCats} cats left.");
            con.WriteLine(ConsoleColor.Green, $"{_currentDogs} dogs left.");
        }

    }
}
