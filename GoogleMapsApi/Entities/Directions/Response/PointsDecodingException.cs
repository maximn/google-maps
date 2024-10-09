using System;

namespace GoogleMapsApi.Entities.Directions.Response
{
	public class PointsDecodingException : Exception
	{
		public string EncodedString { get; set; }

		public PointsDecodingException()
		{
		}

		public PointsDecodingException(string message) : base(message)
		{
		}

		public PointsDecodingException(string message, string encodedString, Exception inner) : base(message, inner)
		{
			EncodedString = encodedString;
		}
	}
}