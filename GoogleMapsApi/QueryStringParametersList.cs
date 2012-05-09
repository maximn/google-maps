using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleMapsApi
{
	public class QueryStringParametersList : List<KeyValuePair<string, string>>
	{
		public void Add(string key, string value)
		{
			Add(new KeyValuePair<string, string>(key, value));
		}

		public override string ToString()
		{
			return "?" + string.Join("&", this.Select(p => Uri.EscapeDataString(p.Key) + "=" + Uri.EscapeDataString(p.Value)));
		}
	}
}
