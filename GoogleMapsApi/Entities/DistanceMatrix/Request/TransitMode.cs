namespace GoogleMapsApi.Entities.DistanceMatrix.Request
{
	using System.Runtime.Serialization;

	[DataContract]
	public enum DistanceMatrixTransitModes
	{
		// To be used for 'transit' travel mode only
		[EnumMember]
		bus, // route should prefer travel by bus
		[EnumMember]
		subway, // route should prefer travel by subway
		[EnumMember]
		train, // route should prefer travel by train
		[EnumMember]
		tram, // route should prefer travel by tram or light rail
		[EnumMember]
		rail, // route should prefer travel by train, tram, light rail and subway
	}
}