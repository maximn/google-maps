using System.Runtime.Serialization;
using GoogleMapsApi.Entities.Common;

namespace GoogleMapsApi.Entities.Geocoding.Response
{
	[DataContract]
	public class FramedLocation
	{
		[DataMember(Name = "southwest")]
		public Location? SouthWest { get; set; }

		[DataMember(Name = "northeast")]
        public Location? NorthEast { get; set; }
        
	    public Location Center
	    {
	        get
	        {
	            if (SouthWest == null || NorthEast == null)
                    return null!;
	            return new Location((SouthWest.Latitude + NorthEast.Latitude) / 2, (SouthWest.Longitude + NorthEast.Longitude) / 2);
	        }
	    }

    }
}
