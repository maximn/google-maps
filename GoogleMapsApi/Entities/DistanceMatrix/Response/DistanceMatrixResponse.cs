namespace GoogleMapsApi.Entities.DistanceMatrix.Response
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	using GoogleMapsApi.Entities.Common;
	using GoogleMapsApi.Entities.DistanceMatrix.Request;

	[DataContract(Name = "DistanceMatrixResponse")]
	public class DistanceMatrixResponse: IResponseFor<DistanceMatrixRequest>
	{
		[DataMember(Name = "status")]
		public string StatusStr
		{
			get
			{
				return Status.ToString();
			}
			set
			{
				Status = (DistanceMatrixStatusCodes)Enum.Parse(typeof(DistanceMatrixStatusCodes), value);
			}
		}

		/// <summary>
		/// "status" contains metadata on the request. See Status Codes below.
		/// </summary>
		public DistanceMatrixStatusCodes Status { get; set; }

		[DataMember(Name = "rows")]
		public IEnumerable<Row> Rows { get; set; }

		[DataMember(Name = "destination_addresses")]
		public IEnumerable<string> DestinationAddresses { get; set; }


		[DataMember(Name = "origin_addresses")]
		public IEnumerable<string> OriginAddresses { get; set; }
	}
}