using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Places.Request;
using GoogleMapsApi.Entities.Places.Response;

namespace GoogleMapsApi.Engine
{
	public class PlacesEngine : MapsAPIGenericEngine<PlacesRequest, PlacesResponse>
	{
		protected override string BaseUrl
		{
			get
			{
				return "maps.googleapis.com/maps/api/place/search/";
			}
		}

		public IAsyncResult BeginGetPlaces(PlacesRequest request, AsyncCallback asyncCallback, object state)
		{
			return BeginQueryGoogleAPI(request, asyncCallback, state);
		}

		public PlacesResponse EndGetPlaces(IAsyncResult asyncResult)
		{
			return EndQueryGoogleAPI(asyncResult);
		}

		public PlacesResponse GetPlaces(PlacesRequest request)
		{
			return QueryGoogleAPI(request);
		}

		public Task<PlacesResponse> GetPlacesAsync(PlacesRequest request)
		{
			return QueryGoogleAPIAsync(request);
		}

		protected override void ConfigureUnderlyingWebClient(WebClient wc, MapsBaseRequest baseRequest)
		{
			PlacesRequest request = (PlacesRequest)baseRequest;

			NameValueCollection queryString = wc.QueryString;

			if (request.Location != null)
			{
				queryString.Add("location", request.Location.LocationString);
			}
			else
			{
				throw new ArgumentException("Location must be povided", "Location");
			}

			if (request.Radius.HasValue)
			{
				double radius = request.Radius.Value;

				if (radius > 50000 || radius<1)
				{
					throw new ArgumentException("Radius must be in range (1, 50000)", "Radius");
				}

				queryString.Add("radius", request.Radius.Value.ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				if (request.RankBy != RankBy.Distance)
				{
					throw new ArgumentException("Radius must be specified unless RankBy is 'Distance'", "Radius");
				}
			}

			if (!string.IsNullOrWhiteSpace(request.Keyword))
			{
				queryString.Add("keyword", request.Keyword);
			}

			if (!string.IsNullOrWhiteSpace(request.Language))
			{
				queryString.Add("language", request.Language);
			}


			if (!string.IsNullOrWhiteSpace(request.Types))
			{
				queryString.Add("types", request.Types);
			}

			if (!string.IsNullOrWhiteSpace(request.Name))
			{
				queryString.Add("name", request.Name);
			}

			switch (request.RankBy)
			{
				case RankBy.Prominence:
					break;
				case RankBy.Distance:
					queryString.Add("rankby", "distance");
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}


			queryString.Add("sensor", request.Sensor.ToString().ToLower());

			if (!string.IsNullOrWhiteSpace(request.ApiKey))
			{
				queryString.Add("key", request.ApiKey);
			}
			else
			{
				throw new ArgumentException("ApiKey must be povided", "ApiKey");
			}
		}
	}
}