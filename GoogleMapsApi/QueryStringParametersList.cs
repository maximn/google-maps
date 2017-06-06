using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleMapsApi
{
	public class QueryStringParametersList
	{
		private List<KeyValuePair<string,string>> List { get; }

		public QueryStringParametersList()
		{
			List = new List<KeyValuePair<string, string>>();
		}

		public void Add(string key, string value)
		{
			List.Add(new KeyValuePair<string, string>(key, value));
		}

		public string GetQueryStringPostfix()
		{
			return string.Join("&", List.Select(p => Uri.EscapeDataString(p.Key) + "=" + Uri.EscapeDataString(p.Value)));
		}
	}
}
