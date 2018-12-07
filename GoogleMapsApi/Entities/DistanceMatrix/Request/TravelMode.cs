namespace GoogleMapsApi.Entities.DistanceMatrix.Request
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum DistanceMatrixTravelModes    {
        [EnumMember]
        driving, // uses road network.
        [EnumMember]
        walking, // uses pedestrian paths (where available).
        [EnumMember]
        bicycling, // uses bicycle paths and preferred streets (where available).
        [EnumMember]
        transit, // uses public transit routes (where available).
    }
}
