using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace ExcludeFieldsNETSerilog
{
	class ExcludeFields
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public int EmployeeID { get; set; }
		public string Designation { get; set; }
		public string Address { get; set; }

		public string excludeFields(string jsonString, List<string> discardedFields)
		{
			JObject jo = JObject.Parse(jsonString);
			foreach (var discardedField in discardedFields)
			{
				jo.Remove(discardedField);
			}
			return jo.ToString(Newtonsoft.Json.Formatting.None);
		}
	}
}
