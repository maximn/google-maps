using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Engine.JsonConverters;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace GoogleMapsApi.Engine
{
    public delegate Uri UriCreatedDelegate(Uri uri);
    public delegate void RawResponseReceivedDelegate(byte[] data);

    public abstract class MapsAPIGenericEngine<TRequest, TResponse>
		where TRequest : MapsBaseRequest, new()
		where TResponse : IResponseFor<TRequest>
	{
        internal static event UriCreatedDelegate OnUriCreated;
        internal static event RawResponseReceivedDelegate OnRawResponseReceived;

		private static readonly HttpClient client = new HttpClient();
		private static readonly JsonSerializerOptions jsonOptions = CreateJsonOptions();

		private static JsonSerializerOptions CreateJsonOptions()
		{
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};

			// Add custom converters
			options.Converters.Add(new EnumMemberJsonConverterFactory());
			options.Converters.Add(new PriceLevelJsonConverter());
			options.Converters.Add(new OverviewPolylineJsonConverter());
			
			// Add Duration converters for specific types
			options.Converters.Add(new DurationJsonConverter<GoogleMapsApi.Entities.DistanceMatrix.Response.Duration>());
			options.Converters.Add(new DurationJsonConverter<GoogleMapsApi.Entities.Directions.Response.Duration>());

			return options;
		}

		protected internal static async Task<TResponse> QueryGoogleAPIAsync(TRequest request, TimeSpan timeout, CancellationToken token = default)
		{
			if (request == null)
				throw new ArgumentNullException(nameof(request));

            var requstUri = request.GetUri();
            var uri = OnUriCreated?.Invoke(requstUri) ?? requstUri;
            
		    var response = await client.DownloadDataTaskAsyncAsString(uri, timeout, token).ConfigureAwait(false);

            OnRawResponseReceived?.Invoke(Encoding.UTF8.GetBytes(response));

            return JsonSerializer.Deserialize<TResponse>(response, jsonOptions);
		}

    }
}