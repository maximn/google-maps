using System.Text.Json.Serialization;

namespace GoogleMapsApi.Entities.AddressValidation.Response
{
    /// <summary>
    /// USPS CASS-processed data for US addresses. Populated only when
    /// <see cref="GoogleMapsApi.Entities.AddressValidation.Request.AddressValidationRequest.EnableUspsCass"/>
    /// is true and the region is US/PR.
    /// </summary>
    public sealed class UspsData
    {
        /// <summary>USPS-standardized form of the address.</summary>
        [JsonPropertyName("standardizedAddress")]
        public UspsAddress? StandardizedAddress { get; set; }

        /// <summary>USPS delivery point code (two digits).</summary>
        [JsonPropertyName("deliveryPointCode")]
        public string? DeliveryPointCode { get; set; }

        /// <summary>Check digit for the delivery point code.</summary>
        [JsonPropertyName("deliveryPointCheckDigit")]
        public string? DeliveryPointCheckDigit { get; set; }

        /// <summary>DPV confirmation indicator (e.g. <c>"Y"</c>, <c>"N"</c>, <c>"S"</c>, <c>"D"</c>).</summary>
        [JsonPropertyName("dpvConfirmation")]
        public string? DpvConfirmation { get; set; }

        /// <summary>Concatenated DPV footnote codes.</summary>
        [JsonPropertyName("dpvFootnote")]
        public string? DpvFootnote { get; set; }

        /// <summary>CMRA (Commercial Mail Receiving Agency) flag.</summary>
        [JsonPropertyName("dpvCmra")]
        public string? DpvCmra { get; set; }

        /// <summary>Whether the address is vacant.</summary>
        [JsonPropertyName("dpvVacant")]
        public string? DpvVacant { get; set; }

        /// <summary>USPS NoStat indicator.</summary>
        [JsonPropertyName("dpvNoStat")]
        public string? DpvNoStat { get; set; }

        /// <summary>Carrier route code.</summary>
        [JsonPropertyName("carrierRoute")]
        public string? CarrierRoute { get; set; }

        /// <summary>Carrier route indicator.</summary>
        [JsonPropertyName("carrierRouteIndicator")]
        public string? CarrierRouteIndicator { get; set; }

        /// <summary>True if EWS (Early Warning System) returned no match for this address.</summary>
        [JsonPropertyName("ewsNoMatch")]
        public bool EwsNoMatch { get; set; }

        /// <summary>Post office city name.</summary>
        [JsonPropertyName("postOfficeCity")]
        public string? PostOfficeCity { get; set; }

        /// <summary>Post office state abbreviation.</summary>
        [JsonPropertyName("postOfficeState")]
        public string? PostOfficeState { get; set; }

        /// <summary>USPS abbreviated city name.</summary>
        [JsonPropertyName("abbreviatedCity")]
        public string? AbbreviatedCity { get; set; }

        /// <summary>FIPS county code.</summary>
        [JsonPropertyName("fipsCountyCode")]
        public string? FipsCountyCode { get; set; }

        /// <summary>County name.</summary>
        [JsonPropertyName("county")]
        public string? County { get; set; }

        /// <summary>USPS eLOT number.</summary>
        [JsonPropertyName("elotNumber")]
        public string? ElotNumber { get; set; }

        /// <summary>USPS eLOT flag (ascending or descending).</summary>
        [JsonPropertyName("elotFlag")]
        public string? ElotFlag { get; set; }

        /// <summary>LACSLink return code.</summary>
        [JsonPropertyName("lacsLinkReturnCode")]
        public string? LacsLinkReturnCode { get; set; }

        /// <summary>LACSLink indicator.</summary>
        [JsonPropertyName("lacsLinkIndicator")]
        public string? LacsLinkIndicator { get; set; }

        /// <summary>True if the postal code is PO-box-only.</summary>
        [JsonPropertyName("poBoxOnlyPostalCode")]
        public bool PoBoxOnlyPostalCode { get; set; }

        /// <summary>SuiteLink footnote.</summary>
        [JsonPropertyName("suitelinkFootnote")]
        public string? SuitelinkFootnote { get; set; }

        /// <summary>Private mailbox designator (e.g. <c>"PMB"</c>).</summary>
        [JsonPropertyName("pmbDesignator")]
        public string? PmbDesignator { get; set; }

        /// <summary>Private mailbox number.</summary>
        [JsonPropertyName("pmbNumber")]
        public string? PmbNumber { get; set; }

        /// <summary>USPS address record type (e.g. <c>"F"</c>, <c>"G"</c>, <c>"H"</c>, <c>"P"</c>, <c>"R"</c>, <c>"S"</c>).</summary>
        [JsonPropertyName("addressRecordType")]
        public string? AddressRecordType { get; set; }

        /// <summary>True if a default address was returned (i.e. one matching multiple inputs).</summary>
        [JsonPropertyName("defaultAddress")]
        public bool DefaultAddress { get; set; }

        /// <summary>USPS error description when CASS could not process the address.</summary>
        [JsonPropertyName("errorMessage")]
        public string? ErrorMessage { get; set; }

        /// <summary>True if the address was successfully CASS-processed.</summary>
        [JsonPropertyName("cassProcessed")]
        public bool CassProcessed { get; set; }
    }

    /// <summary>USPS-standardized address form.</summary>
    public sealed class UspsAddress
    {
        /// <summary>First address line (street).</summary>
        [JsonPropertyName("firstAddressLine")]
        public string? FirstAddressLine { get; set; }

        /// <summary>Firm/organization line.</summary>
        [JsonPropertyName("firm")]
        public string? Firm { get; set; }

        /// <summary>Second address line (apartment, suite).</summary>
        [JsonPropertyName("secondAddressLine")]
        public string? SecondAddressLine { get; set; }

        /// <summary>Puerto Rico urbanization name.</summary>
        [JsonPropertyName("urbanization")]
        public string? Urbanization { get; set; }

        /// <summary>Combined city/state/ZIP line.</summary>
        [JsonPropertyName("cityStateZipAddressLine")]
        public string? CityStateZipAddressLine { get; set; }

        /// <summary>City.</summary>
        [JsonPropertyName("city")]
        public string? City { get; set; }

        /// <summary>State (USPS two-letter abbreviation).</summary>
        [JsonPropertyName("state")]
        public string? State { get; set; }

        /// <summary>5-digit ZIP code.</summary>
        [JsonPropertyName("zipCode")]
        public string? ZipCode { get; set; }

        /// <summary>ZIP+4 extension.</summary>
        [JsonPropertyName("zipCodeExtension")]
        public string? ZipCodeExtension { get; set; }
    }
}
