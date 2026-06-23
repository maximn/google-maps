using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.AirQuality.Request
{
    /// <summary>
    /// The air-quality heatmap type (index and colour scheme) for a
    /// <see cref="HeatmapTileRequest"/>. Determines which pollutant/index the returned PNG tile renders.
    /// </summary>
    public enum AirQualityMapType
    {
        /// <summary>No map type specified. Ignored by the server if sent.</summary>
        [EnumMember(Value = "MAP_TYPE_UNSPECIFIED")] Unspecified,

        /// <summary>Universal AQI, red-green palette.</summary>
        [EnumMember(Value = "UAQI_RED_GREEN")] UaqiRedGreen,

        /// <summary>Universal AQI, indigo-Persian palette.</summary>
        [EnumMember(Value = "UAQI_INDIGO_PERSIAN")] UaqiIndigoPersian,

        /// <summary>PM2.5 concentration, indigo-Persian palette.</summary>
        [EnumMember(Value = "PM25_INDIGO_PERSIAN")] Pm25IndigoPersian,

        /// <summary>UK DEFRA Daily Air Quality Index.</summary>
        [EnumMember(Value = "GBR_DEFRA")] GbrDefra,

        /// <summary>German Federal Environment Agency (UBA) index.</summary>
        [EnumMember(Value = "DEU_UBA")] DeuUba,

        /// <summary>Environment Canada Air Quality Health Index.</summary>
        [EnumMember(Value = "CAN_EC")] CanEc,

        /// <summary>French ATMO index.</summary>
        [EnumMember(Value = "FRA_ATMO")] FraAtmo,

        /// <summary>US EPA Air Quality Index.</summary>
        [EnumMember(Value = "US_AQI")] UsAqi,
    }
}
