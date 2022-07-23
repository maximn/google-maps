using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi
{
	public class RouteMapGenerationResult
	{
		public string URL { get; set; }

		
		public RouteMapGenerationResult(string url)
		{
			URL = url;

		}
	}
}
