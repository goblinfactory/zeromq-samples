using System;
using Konsole;
using NetMQ;
using NetMQ.Sockets;

namespace ZeroMq.Samples
{

    public class RequestEcho
    {
        private readonly IConsole _con;
        private readonly ResponseSocket _socket;
        private readonly ConsoleColor _reqColor;
        private readonly ConsoleColor _resColor;

        public RequestEcho(IConsole con, ResponseSocket socket, ConsoleColor reqColor, ConsoleColor resColor)
        {
            _con = con;
            _socket = socket;
            _reqColor = reqColor;
            _resColor = resColor;
        }

        public string Request(string request)
        {
            _con.Write("request  : ");
            _con.WriteLine(_reqColor, request);
            _socket.SendFrame(request);
            var response = _socket.ReceiveFrameString();
            _con.WriteLine(_resColor, response);
            return response;
        }

    }
}