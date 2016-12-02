using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace log4net_loggly_sample_app
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetLogger(typeof(Program));
            //Send plaintext
            logger.Info("your log message");

            //Send an exception
            logger.Error("your log message", new Exception("your exception message"));

            //Send a JSON object
            var items = new Dictionary<string, string>();
            items.Add("key1", "value1");
            items.Add("key2", "value2");
            logger.Info(items);
            
            //Wait atleast 5 seconds before terminating the application to send logs to loggly
            Console.WriteLine("Log is sending to loggly. Wait atleast 5 seconds before terminating the application.");
            Console.ReadKey();
        }
    }
}
