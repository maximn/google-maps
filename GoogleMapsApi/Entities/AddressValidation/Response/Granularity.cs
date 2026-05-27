using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.AddressValidation.Response
{
    /// <summary>
    /// Granularity at which an address (or its inferred geocode) is understood. Used by both the
    /// <c>inputGranularity</c>, <c>validationGranularity</c>, and <c>geocodeGranularity</c> verdict fields.
    /// </summary>
    public enum Granularity
    {
        /// <summary>Default value. The granularity is unspecified.</summary>
        [EnumMember(Value = "GRANULARITY_UNSPECIFIED")] Unspecified,

        /// <summary>Below-building level result (apartment, suite).</summary>
        [EnumMember(Value = "SUB_PREMISE")] SubPremise,

        /// <summary>Building-level result.</summary>
        [EnumMember(Value = "PREMISE")] Premise,

        /// <summary>Approximation of the building, e.g. a corner or nearby landmark.</summary>
        [EnumMember(Value = "PREMISE_PROXIMITY")] PremiseProximity,

        /// <summary>Block-level result (Japan only).</summary>
        [EnumMember(Value = "BLOCK")] Block,

        /// <summary>Street-level result.</summary>
        [EnumMember(Value = "ROUTE")] Route,

        /// <summary>All other levels (e.g. locality).</summary>
        [EnumMember(Value = "OTHER")] Other,
    }
}
