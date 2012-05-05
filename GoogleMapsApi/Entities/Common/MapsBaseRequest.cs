namespace GoogleMapsApi.Entities.Common
{
	public class MapsBaseRequest
	{
		/// <summary>
		/// sensor (required) — Indicates whether or not the directions request comes from a device with a location sensor. This value must be either true or false.
		/// </summary>
		public bool Sensor { get; set; } //Required

		public virtual bool IsSSL { get; set; }
	}
}
