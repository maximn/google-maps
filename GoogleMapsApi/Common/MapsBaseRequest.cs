using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoogleMapsApi.Common
{
	public class MapsBaseRequest
	{
		public ResponseOutputType Output { get; set; }

		/// <summary>
		/// sensor (required) — Indicates whether or not the directions request comes from a device with a location sensor. This value must be either true or false.
		/// </summary>
		public bool Sensor { get; set; } //Required
	}
}
