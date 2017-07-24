using System;
using Konsole;

namespace ZeroMq.Samples
{
    public class KeyHandler<T> 
    {
        public char Key { get; }
        public Action<IConsole, T> Action { get; }

        public KeyHandler(char key, Action<IConsole, T> action)
        {
            Key = key;
            Action = action;
        }
    }
}