namespace GoogleMapsApi.Entities.DistanceMatrix.Request
{

    public enum DistanceMatrixTransitRoutingPreferences
    {
        // To be used for 'transit' travel mode only
        less_walking, // route should prefer limited amounts of walking
        fewer_transfers, // route should prefer limited numbers of transfers
    }
}
