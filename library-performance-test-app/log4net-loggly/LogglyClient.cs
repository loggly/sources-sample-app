using System;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace log4net.loggly
{

    public class LogglyClient : ILogglyClient
    {
        static float totalThroughPutInKB = 0;
        static int grossRetryCount = 0;
        static int failureLogCount = 0;
        static float eventsPerSeconds = 0;
        static int logLost = 0;
        static float totalTimeInSeconds = 0;
        static int successLogCount = 0;

        public static string getLogStatus()
        {

            var logStatus = new
            {
                ThroughputInKB = totalThroughPutInKB,
                EventPerSecond = eventsPerSeconds,
                Success = successLogCount,
                Failure = failureLogCount,
                Retries = grossRetryCount,
                LogLost = logLost
            };

            resetLogStatus();
            return Newtonsoft.Json.JsonConvert.SerializeObject(logStatus);
        }
        private static void resetLogStatus()
        {
            totalThroughPutInKB = 0;
            grossRetryCount = 0;
            failureLogCount = 0;
            eventsPerSeconds = 0;
            logLost = 0;
            totalTimeInSeconds = 0;
            successLogCount = 0;
        }
        public virtual void Send(ILogglyAppenderConfig config, string message)
        {
            int maxRetryAllowed = 5;
            int totalRetries = 0;
            DateTime startTime = DateTime.UtcNow;
            DateTime endTime = DateTime.UtcNow;

            string _tag = config.Tag;

            //keeping userAgent backward compatible
            if (!string.IsNullOrWhiteSpace(config.UserAgent))
            {
                _tag = _tag + "," + config.UserAgent;
            }

            while (totalRetries < maxRetryAllowed)
            {
                totalRetries++;

                if (totalRetries > 1)
                    grossRetryCount++;
                try
                {
                    var bytes = Encoding.UTF8.GetBytes(message);
                    var webRequest = CreateWebRequest(config, _tag);
                    using (var dataStream = webRequest.GetRequestStream())
                    {
                        dataStream.Write(bytes, 0, bytes.Length);
                        dataStream.Flush();
                        dataStream.Close();
                    }

                    var webResponse = webRequest.GetResponse();
                    webResponse.Close();

                    // Some calculation to count event per sec, total throughput in kb
                    successLogCount++;
                    endTime = DateTime.UtcNow;
                    totalTimeInSeconds = (float)((endTime - startTime).TotalSeconds);
                    eventsPerSeconds = successLogCount / totalTimeInSeconds;
                    totalThroughPutInKB = totalThroughPutInKB + ((message.Length * 2) / 1024f);
                    break;
                }
                catch
                {
                    failureLogCount++;
                    if (totalRetries == maxRetryAllowed)
                        logLost++;
                }
            }
        }

        protected virtual HttpWebRequest CreateWebRequest(ILogglyAppenderConfig config, string tag)
        {
            var url = String.Concat(config.RootUrl, config.LogMode, config.InputKey);
            //adding userAgent as tag in the log
            url = String.Concat(url, "/tag/" + tag);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ReadWriteTimeout = request.Timeout = config.TimeoutInSeconds * 1000;
            request.UserAgent = config.UserAgent;
            request.KeepAlive = true;
            request.ContentType = "application/json";
            return request;
        }
    }
}
