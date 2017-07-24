using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroMq.Samples.Chapter1
{
    class Program
    {
        public static void Run()
        {
            new Demo("Send and recieve",
                (c, k) => new Server(c, k).Run(),
                (c, k) => new AnimalClient(c, k).Run()
            ).Run();
        }
    }
}
