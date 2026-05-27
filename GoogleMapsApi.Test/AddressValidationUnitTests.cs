using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.AddressValidation.Request;
using GoogleMapsApi.Entities.AddressValidation.Response;
using NUnit.Framework;

namespace GoogleMapsApi.Test
{
    [TestFixture]
    public class AddressValidationUnitTests
    {
        [Test]
        public void GetUri_TargetsAddressValidationHost_WithKeyInQueryString()
        {
            var request = new AddressValidationRequest { ApiKey = "abc 123" };

            var uri = request.GetUri();

            Assert.That(uri.Host, Is.EqualTo("addressvalidation.googleapis.com"));
            Assert.That(uri.AbsolutePath, Is.EqualTo("/v1:validateAddress"));
            Assert.That(uri.Query, Is.EqualTo("?key=abc%20123"));
        }

        [Test]
        public void GetUri_MissingApiKey_Throws()
        {
            var request = new AddressValidationRequest();
            Assert.Throws<InvalidOperationException>(() => request.GetUri());
        }

        [Test]
        public async Task QueryAsync_IssuesPost_WithJsonBody_AndCanonicalAddressFields()
        {
            var canned = "{\"result\":{\"verdict\":{\"validationGranularity\":\"PREMISE\",\"addressComplete\":true}}," +
                         "\"responseId\":\"abc-1\"}";
            var handler = new RecordingHandler(canned);
            using var http = new HttpClient(handler);

            var request = new AddressValidationRequest
            {
                ApiKey = "k",
                EnableUspsCass = true,
                PreviousResponseId = "prev-1",
                Address = new PostalAddress
                {
                    RegionCode = "US",
                    AddressLines = new System.Collections.Generic.List<string> { "1600 Amphitheatre Pkwy" },
                    Locality = "Mountain View",
                    AdministrativeArea = "CA",
                    PostalCode = "94043",
                },
            };

            var response = await MapsAPIGenericEngine<AddressValidationRequest, AddressValidationResponse>
                .QueryGoogleAPIAsync(http, request, TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);

            Assert.That(handler.LastMethod, Is.EqualTo(HttpMethod.Post));
            Assert.That(handler.LastContentType, Is.EqualTo("application/json"));
            Assert.That(handler.LastUri!.Host, Is.EqualTo("addressvalidation.googleapis.com"));

            using var body = JsonDocument.Parse(handler.LastRequestBody!);
            var root = body.RootElement;

            Assert.That(root.GetProperty("address").GetProperty("regionCode").GetString(), Is.EqualTo("US"));
            Assert.That(root.GetProperty("address").GetProperty("addressLines")[0].GetString(), Is.EqualTo("1600 Amphitheatre Pkwy"));
            Assert.That(root.GetProperty("address").GetProperty("locality").GetString(), Is.EqualTo("Mountain View"));
            Assert.That(root.GetProperty("enableUspsCass").GetBoolean(), Is.True);
            Assert.That(root.GetProperty("previousResponseId").GetString(), Is.EqualTo("prev-1"));

            Assert.That(response.ResponseId, Is.EqualTo("abc-1"));
            Assert.That(response.Result!.Verdict!.ValidationGranularity, Is.EqualTo(Granularity.Premise));
            Assert.That(response.Result!.Verdict!.AddressComplete, Is.True);
        }

        [Test]
        public async Task QueryAsync_OmitsOptionalFields_WhenNotSet()
        {
            var handler = new RecordingHandler("{\"responseId\":\"x\"}");
            using var http = new HttpClient(handler);

            var request = new AddressValidationRequest
            {
                ApiKey = "k",
                Address = new PostalAddress { RegionCode = "US" },
            };

            await MapsAPIGenericEngine<AddressValidationRequest, AddressValidationResponse>
                .QueryGoogleAPIAsync(http, request, TimeSpan.FromMilliseconds(-1), CancellationToken.None, null, null);

            using var body = JsonDocument.Parse(handler.LastRequestBody!);
            var root = body.RootElement;

            Assert.That(root.TryGetProperty("enableUspsCass", out _), Is.False, "EnableUspsCass=false should be omitted");
            Assert.That(root.TryGetProperty("previousResponseId", out _), Is.False);
            Assert.That(root.TryGetProperty("languageOptions", out _), Is.False);
            Assert.That(root.TryGetProperty("sessionToken", out _), Is.False);
        }

        [Test]
        public void Response_DeserializesEnumsWithEnumMemberValues()
        {
            var json = "{\"result\":{\"verdict\":{\"inputGranularity\":\"SUB_PREMISE\"," +
                       "\"validationGranularity\":\"PREMISE_PROXIMITY\"," +
                       "\"geocodeGranularity\":\"OTHER\"," +
                       "\"addressComplete\":false," +
                       "\"hasUnconfirmedComponents\":true}," +
                       "\"address\":{\"addressComponents\":[" +
                       "{\"componentType\":\"locality\",\"confirmationLevel\":\"CONFIRMED\"}," +
                       "{\"componentType\":\"postal_code\",\"confirmationLevel\":\"UNCONFIRMED_AND_SUSPICIOUS\"}" +
                       "]}}}";

            var options = JsonSerializerConfiguration.CreateOptions();
            var response = JsonSerializer.Deserialize<AddressValidationResponse>(json, options);

            Assert.That(response, Is.Not.Null);
            Assert.That(response!.Result!.Verdict!.InputGranularity, Is.EqualTo(Granularity.SubPremise));
            Assert.That(response.Result.Verdict.ValidationGranularity, Is.EqualTo(Granularity.PremiseProximity));
            Assert.That(response.Result.Verdict.GeocodeGranularity, Is.EqualTo(Granularity.Other));
            Assert.That(response.Result.Verdict.HasUnconfirmedComponents, Is.True);
            Assert.That(response.Result.Address!.AddressComponents![0].ConfirmationLevel, Is.EqualTo(ConfirmationLevel.Confirmed));
            Assert.That(response.Result.Address.AddressComponents[1].ConfirmationLevel, Is.EqualTo(ConfirmationLevel.UnconfirmedAndSuspicious));
        }

        private sealed class RecordingHandler : HttpMessageHandler
        {
            private readonly string _responseJson;

            public RecordingHandler(string responseJson) { _responseJson = responseJson; }

            public HttpMethod? LastMethod { get; private set; }
            public Uri? LastUri { get; private set; }
            public string? LastRequestBody { get; private set; }
            public string? LastContentType { get; private set; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                LastMethod = request.Method;
                LastUri = request.RequestUri;
                if (request.Content != null)
                {
                    LastRequestBody = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                    LastContentType = request.Content.Headers.ContentType?.MediaType;
                }
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(_responseJson, Encoding.UTF8, "application/json")
                };
            }
        }
    }
}
