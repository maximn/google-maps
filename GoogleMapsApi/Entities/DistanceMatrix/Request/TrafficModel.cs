namespace GoogleMapsApi.Entities.DistanceMatrix.Request
{
	using System.Runtime.Serialization;

	[DataContract]
	public enum DistanceMatrixTrafficModels
	{
		// To be used for 'driving' travel mode only
		[EnumMember]
		best_guess, // uses historical trafic conditions. live traffic weighted more significantly for departure times closer to now.
		[EnumMember]
		pessimistic, // returned duration in traffic should be longer than actual travel on most days.
		[EnumMember]
		optimistic, // returned duration in traffic should be shorter than actual travel on most days.
	}
}