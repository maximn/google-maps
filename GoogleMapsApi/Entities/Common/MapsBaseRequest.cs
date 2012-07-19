using System;
using System.Linq;

namespace GoogleMapsApi.Entities.Common
{
	public abstract class MapsBaseRequest
	{
		/// <summary>
		/// True to indicate that request comes from a device with a location sensor, otherwise false. 
		/// This information is required by Google and does not affect the results.
		/// </summary>
		/// <remarks>
		/// It is unclear if Google refers to the device performing the request or the source of the location data.
		/// In the geocoding API reference at https://developers.google.com/maps/documentation/geocoding/ the definition is:
		/// 
		///     sensor (required) — Indicates whether or not the geocoding request comes from a device with a location sensor.
		/// 
		/// This implies that only mobile devices that are equipped with a location sensor (such as GPS) should set the Sensor value 
		/// to True. So if a location is sent to a web server which then calls the Google API, it apparently should set the Sensor
		/// value to false, since the web server isn't a location sensor equipped device.
		/// 
		/// In another page of their documentation, https://developers.google.com/maps/documentation/javascript/tutorial they say:
		/// 
		///     The sensor parameter of the URL must be included, and indicates whether this application uses a sensor (such as a GPS 
		///     locator) to determine the user's location.
		/// 
		/// This implies something completely different, that the Sensor value must be set to true if the source of the location
		/// information is a sensor or not, regardless if the request is being performed by a location sensor equipped device or not.
		/// </remarks>
		public bool Sensor { get; set; }

		/// <summary>
		/// True to use use the https protocol; false to use http. The default is false.
		/// </summary>
		public virtual bool IsSSL { get; set; }

		protected internal virtual string BaseUrl
		{
			get
			{
				return "maps.google.com/maps/api/";
			}
		}

		protected virtual QueryStringParametersList GetQueryStringParameters()
		{
			return new QueryStringParametersList { { "sensor", Sensor.ToString().ToLower() } };
		}

		public virtual Uri GetUri()
		{
			string scheme = IsSSL ? "https://" : "http://";
			var escapedParameters = GetQueryStringParameters().Select(p => Uri.EscapeDataString(p.Key) + "=" + Uri.EscapeDataString(p.Value));
			var queryString = string.Join("&", escapedParameters);
			return new Uri(scheme + BaseUrl + "json?" + queryString);
		}
	}
}
