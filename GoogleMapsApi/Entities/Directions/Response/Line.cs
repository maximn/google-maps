using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.Directions.Response
{
	[DataContract(Name = "line")]
	public class Line
	{
		/// <summary>
		/// Contains the full name of this transit line. eg. "7 Avenue Express".
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// Contains the short name of this transit line. This will normally be a line number, such as "M7" or "355".
		/// </summary>
		[DataMember(Name = "short_name")]
		public string ShortName { get; set; }

		/// <summary>
		/// Contains the color commonly used in signage for this transit line. The color will be specified as a hex string such as: #FF0033. 
		/// </summary>
		[DataMember(Name = "color")]
		public string Color { get; set; }

		/// <summary>
		/// Contains a List of TransitAgency objects that each provide information about the operator of the line.
		/// </summary>
		[DataMember(Name = "agencies")]
		public List<TransitAgency> Agencies { get; set; }

		/// <summary>
		/// Contains the URL for this transit line as provided by the transit agency.
		/// </summary>
		[DataMember(Name = "url")]
		public string Url { get; set; }

		/// <summary>
		/// Contains the URL for the icon associated with this line.
		/// </summary>
		[DataMember(Name = "icon")]
		public string Icon { get; set; }

		/// <summary>
		/// Contains the color of text commonly used for signage of this line. The color will be specified as a hex string.
		/// </summary>
		[DataMember(Name = "text_color")]
		public string TextColor { get; set; }

		/// <summary>
		/// Contains the type of vehicle used on this line.
		/// </summary>
		[DataMember(Name = "vehicle")]
		public Vehicle Vehicle { get; set; }
	}
}