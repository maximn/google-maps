using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.PlacesDetails.Response;

/// <summary>
/// Contains a summary of the place. 
/// </summary>
[DataContract]
public class PlaceEditorialSummary
{
    /// <summary>
    /// The language of the previous fields. May not always be present.
    /// </summary>
    [DataMember(Name = "language")] 
    public string Language { get; set; }
    
    /// <summary>
    /// A medium-length textual summary of the place.
    /// </summary>
    [DataMember(Name = "overview")] 
    public string Overview { get; set; }
}