using Konsole;
using NetMQ.Sockets;
using ZeroMq.Samples.Chapter1;

namespace ZeroMq.Samples
{
    public class ResponseSocketMicroService : MicroService<ResponseSocket>
    {

        public ResponseSocketMicroService(
            int port,
            IConsole con,
            IKeyboard k,
            char quitkey,
            string started,
            string stopped,
            params KeyHandler<ResponseSocket>[] handlers
        )   
            : base(() => new ResponseSocket($"@tcp://*:{port}"), quitkey, k, con, started ?? "server started", stopped ?? "server stopped", handlers) { }
    }
}