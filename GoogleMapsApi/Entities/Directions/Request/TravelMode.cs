using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Directions.Request
{
	public enum TravelMode
	{
		[EnumMember(Value = "DRIVING")]
		Driving,
		[EnumMember(Value = "WALKING")]
		Walking,
		[EnumMember(Value = "BICYCLING")]
		Bicycling,
		[EnumMember(Value = "TRANSIT")]
		Transit
	}
}
