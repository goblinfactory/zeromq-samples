using Konsole;
using NetMQ.Sockets;
using ZeroMq.Samples.Chapter1;

namespace ZeroMq.Samples
{

    public class RequestSocketMicroService : MicroService<RequestSocket>
    {

        public RequestSocketMicroService(
            string host, 
            int port,
            IConsole con,
            IKeyboard k, 
            char quitkey,
            string started,
            string stopped,
            params KeyHandler<RequestSocket>[] handlers
        )  
            : base(()=> new RequestSocket($">tcp://{host}:{port}"), quitkey, k, con, started ?? "client started", stopped ?? "client stopped", handlers) {}
    }
}
