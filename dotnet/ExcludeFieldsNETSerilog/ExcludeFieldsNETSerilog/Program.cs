using Loggly;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;

namespace ExcludeFieldsNETSerilog
{
	class Program
	{
		static void Main(string[] args)
		{
			ExcludeFields ef = new ExcludeFields() { FirstName = "Jack", LastName = "Makan", EmployeeID = 100, Designation = "SSE", Address = "United States" };
			List<string> discardedFieldsList = new List<string>();
			discardedFieldsList.Add("FirstName");
			discardedFieldsList.Add("Designation");
			discardedFieldsList.Add("Address");
			var log = new LoggerConfiguration()
						  .WriteTo.Loggly()
						  .CreateLogger();
			ILogglyClient _loggly = new LogglyClient();
			var logEvent = new LogglyEvent();
			logEvent.Data.Add("message", "Simple message at {0}");
			logEvent.Data.Add("context", ef.excludeFields(JsonConvert.SerializeObject(ef), discardedFieldsList));
			logEvent.Data.Add("Error", new Exception("your exception message"));
			for (int i = 0; i<10; i++) {
				_loggly.Log(logEvent);
			}
			Console.ReadKey();
		}
	}
}
