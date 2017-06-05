namespace GoogleMapsApi.Entities.DistanceMatrix.Request
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum DistanceMatrixRestrictions
    {
        [EnumMember]
        tolls,
        [EnumMember]
        highways,
        [EnumMember]
        ferries,
        [EnumMember]
        indoor,
    }
}
