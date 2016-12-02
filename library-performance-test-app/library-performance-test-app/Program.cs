using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.loggly;

namespace library_performance_test_app
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetLogger(typeof(Program));
            Timer t = new Timer(printLogStatus, null, 0, 3 * 1000);
            for (int i = 0; i < 20000; i++)
            {
                logger.Info(i + ":   Hi to all Serialization using Json.NET is even easier. In this next sample, a Dictionary of strings (similar to the one used above for deserialization) is declared and then serialized to JSON format.Hi to all Serialization using Json.NET is even easier. In this next sample, a Dictionary of strings (similar to the one used above for deserialization) is declared and then serialized to JSON format.");
            }

            Console.ReadKey();

        }
        static void printLogStatus(Object o)
        {
            Console.WriteLine(LogglyClient.getLogStatus());
            GC.Collect();
        }
    }
}
