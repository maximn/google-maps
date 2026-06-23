using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.AirQuality.Request
{
    /// <summary>
    /// Optional additional computations the Air Quality API can run for a request, each enriching the
    /// response with extra data (local AQIs, health advice, pollutant detail). Supplied via
    /// <c>extraComputations</c>.
    /// </summary>
    public enum ExtraComputation
    {
        /// <summary>No extra computation. Ignored by the server if sent.</summary>
        [EnumMember(Value = "EXTRA_COMPUTATION_UNSPECIFIED")] Unspecified,

        /// <summary>Include the local (national) Air Quality Index for the requested location.</summary>
        [EnumMember(Value = "LOCAL_AQI")] LocalAqi,

        /// <summary>Include health recommendations for the general population and at-risk groups.</summary>
        [EnumMember(Value = "HEALTH_RECOMMENDATIONS")] HealthRecommendations,

        /// <summary>Include supplementary sources/effects information for each pollutant.</summary>
        [EnumMember(Value = "POLLUTANT_ADDITIONAL_INFO")] PollutantAdditionalInfo,

        /// <summary>Include concentrations of the dominant pollutants identified by the indexes.</summary>
        [EnumMember(Value = "DOMINANT_POLLUTANT_CONCENTRATION")] DominantPollutantConcentration,

        /// <summary>Include concentrations of every pollutant measured by the indexes.</summary>
        [EnumMember(Value = "POLLUTANT_CONCENTRATION")] PollutantConcentration,
    }
}
