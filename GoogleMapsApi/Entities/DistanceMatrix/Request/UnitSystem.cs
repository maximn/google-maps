namespace GoogleMapsApi.Entities.DistanceMatrix.Request
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum DistanceMatrixUnitSystems
    {
        [EnumMember]
        metric, // kilometers an meters.
        [EnumMember]
        imperial, // miles and feet.
    }
}
