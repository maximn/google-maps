namespace GoogleMapsApi.Entities.DistanceMatrix.Request
{

    public enum DistanceMatrixTravelModes    {
        driving, // uses road network.
        walking, // uses pedestrian paths (where available).
        bicycling, // uses bicycle paths and preferred streets (where available).
        transit, // uses public transit routes (where available).
    }
}
