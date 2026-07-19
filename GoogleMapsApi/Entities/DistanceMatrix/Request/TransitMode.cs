namespace GoogleMapsApi.Entities.DistanceMatrix.Request
{

    public enum DistanceMatrixTransitModes
    {
        // To be used for 'transit' travel mode only
        bus, // route should prefer travel by bus
        subway, // route should prefer travel by subway
        train, // route should prefer travel by train
        tram, // route should prefer travel by tram or light rail
        rail, // route should prefer travel by train, tram, light rail and subway
    }
}
