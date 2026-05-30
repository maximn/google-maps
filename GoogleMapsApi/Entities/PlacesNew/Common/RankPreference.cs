using System.Runtime.Serialization;

namespace GoogleMapsApi.Entities.PlacesNew.Common
{
    /// <summary>
    /// How search results should be ranked. Shared by Text Search and Nearby Search; note that the
    /// default differs by surface (Text Search defaults to <see cref="Relevance"/>, Nearby Search to
    /// <see cref="Distance"/>).
    /// </summary>
    public enum RankPreference
    {
        /// <summary>Rank preference unspecified; the API picks a per-surface default.</summary>
        [EnumMember(Value = "RANK_PREFERENCE_UNSPECIFIED")] Unspecified,

        /// <summary>Rank results by distance from the query location.</summary>
        [EnumMember(Value = "DISTANCE")] Distance,

        /// <summary>Rank results by relevance.</summary>
        [EnumMember(Value = "RELEVANCE")] Relevance,
    }
}
