namespace GoogleMapsApi.StaticMaps.Enums
{
    /// <summary>
    /// Element types that can be styled in Google Maps
    /// Reference: https://developers.google.com/maps/documentation/maps-static/style-reference
    /// </summary>
    public enum MapElementType
    {
        /// <summary>
        /// All elements (default)
        /// </summary>
        All,

        /// <summary>
        /// Geometry elements
        /// </summary>
        Geometry,

        /// <summary>
        /// Geometry fill
        /// </summary>
        GeometryFill,

        /// <summary>
        /// Geometry stroke
        /// </summary>
        GeometryStroke,

        /// <summary>
        /// Labels
        /// </summary>
        Labels,

        /// <summary>
        /// Label icons
        /// </summary>
        LabelsIcon,

        /// <summary>
        /// Label text
        /// </summary>
        LabelsText,

        /// <summary>
        /// Label text fill
        /// </summary>
        LabelsTextFill,

        /// <summary>
        /// Label text stroke
        /// </summary>
        LabelsTextStroke
    }
}
