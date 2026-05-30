using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Response
{
    /// <summary>Payment options accepted by a place.</summary>
    public sealed class PaymentOptions
    {
        /// <summary>Whether credit cards are accepted.</summary>
        [JsonPropertyName("acceptsCreditCards")]
        public bool? AcceptsCreditCards { get; set; }

        /// <summary>Whether debit cards are accepted.</summary>
        [JsonPropertyName("acceptsDebitCards")]
        public bool? AcceptsDebitCards { get; set; }

        /// <summary>Whether only cash is accepted.</summary>
        [JsonPropertyName("acceptsCashOnly")]
        public bool? AcceptsCashOnly { get; set; }

        /// <summary>Whether NFC (contactless) payments are accepted.</summary>
        [JsonPropertyName("acceptsNfc")]
        public bool? AcceptsNfc { get; set; }
    }
}
