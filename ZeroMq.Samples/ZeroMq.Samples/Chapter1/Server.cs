using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Konsole;
using NetMQ;
using NetMQ.Sockets;

namespace ZeroMq.Samples.Chapter1
{
    public class Server 
    {
        private readonly IConsole _con;
        private readonly IKeyboard _k;
        private List<string> _animals = new List<string>();

        public Server(IConsole con, IKeyboard k)
        {
            _con = con;
            _k = k;
        }

        public void Run()
        {
            var uri = "tcp://*:1234";
            _con.WriteLine("'1':ListStock '2':Clearstock '3':start server :'4':stop server 'q':quit.");
            var server = new ResponseSocketMicroService(
                 1234, _con, _k, 'q', null, null,
                new KeyHandler<ResponseSocket>('1', ListStock),
                new KeyHandler<ResponseSocket>('2', ClearStock),
                new KeyHandler<ResponseSocket>('3', startServer),
                new KeyHandler<ResponseSocket>('4', stopServer)
            );
            server.Run();

        }

        private void ListStock(IConsole con, ResponseSocket socket)
        {
            con.WriteLine("-----------");
            con.WriteLine("list stock");
            con.WriteLine("-----------");
            foreach(var animal in _animals) con.WriteLine(animal);
            con.WriteLine("");
        }

        private void ClearStock(IConsole con, ResponseSocket socket)
        {
            _animals.Clear();
            con.WriteLine("animal stock database cleared.\n");
        }

        // https://stackoverflow.com/questions/1156058/how-do-you-interrupt-a-socket-receivefrom-call

        private void startServer(IConsole con, ResponseSocket socket)
        {
            con.WriteLine("client must send [FIN] to stop."); // currently this is the only way!
            string request = "";
            while ((request = socket.ReceiveFrameString())!="[FIN]")
            {
                _animals.Add(request);
                con.WriteLine(request);
                socket.SendFrame($"(OK) {request}");
            }
            socket.SendFrame($"(OK) [FIN]");
        }

        private void stopServer(IConsole con, ResponseSocket socket)
        {
            _animals.Clear();
            con.WriteLine("animal stock database cleared.\n");
        }

    }
}
