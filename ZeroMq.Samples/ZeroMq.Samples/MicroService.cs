using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Konsole;
using ZeroMq.Samples.Chapter1;

namespace ZeroMq.Samples
{
    public class MicroService<T> where T : IDisposable
    {
        private readonly Func<T> _socketHandlerFactory;
        private readonly char _quitKey;
        private readonly IKeyboard _k;
        private readonly IConsole _con;
        private readonly string _started;
        private readonly string _stopped;
        private readonly KeyHandler<T>[] _handlers;
        private bool _stop;

        public MicroService(
            Func<T> socketHandlerFactory, 
            char quitKey, 
            IKeyboard k, 
            IConsole con, 
            string started, 
            string stopped, 
            params KeyHandler<T>[] handlers)
        {
            _socketHandlerFactory = socketHandlerFactory;
            _quitKey = quitKey;
            _k = k;
            _con = con;
            _started = started;
            _stopped = stopped;
            _handlers = handlers;
        }

        public void Run()
        {
            var actions = new ConcurrentQueue<Action<IConsole, T>>();

            using (T socket = _socketHandlerFactory())
            {
                foreach (var handler in _handlers)
                {
                    _k.OnCharPressed(handler.Key,c=> actions.Enqueue(handler.Action));
                }
                _k.OnCharPressed(_quitKey, c => actions.Enqueue((con,soc)=> { _stop = true;}));

                _con.WriteLine(_started);

                while (!_stop)
                {
                    while (!_stop && !actions.IsEmpty)
                    {
                        Action<IConsole, T> action;
                        if (actions.TryDequeue(out action)) action(_con, socket);
                    }
                    Thread.Sleep(1);
                }
            }
            _con.WriteLine(_stopped);
        }

    }
}
