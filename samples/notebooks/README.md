# Interactive notebooks

Live, runnable [Polyglot Notebooks](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.dotnet-interactive-vscode)
(`.dib`) — one per Google Maps API surface. Each notebook references the published
[`GoogleMapsApi`](https://www.nuget.org/packages/GoogleMapsApi) NuGet package (`#r "nuget: GoogleMapsApi, 2.4.0"`),
so they run anywhere without building this repo. Execute the cells top to bottom to call the real API
and see each response rendered inline — a more hands-on alternative to the README's code blocks and the
full project [samples](../).

## How to run

1. Install the **Polyglot Notebooks** extension in VS Code (it pulls in the .NET Interactive kernel),
   or use the [`dotnet repl`](https://github.com/jonsequitur/dotnet-repl) CLI.
2. Set your key once for the session: `export GOOGLE_API_KEY=your-key` (the notebooks read it from the
   environment, matching the other samples). Don't have one? Get it in the
   [Google Cloud Console](https://console.cloud.google.com/) and enable the relevant API.
3. Open a `.dib` file and run the cells in order — the package reference, key, and client are set up in
   the first few cells and reused by the rest.

## Notebooks

### Free tier

| Notebook | API surface | What it shows |
| --- | --- | --- |
| [Geocoding.dib](Geocoding.dib) | Geocoding | Address → coordinates, and reverse geocoding |
| [Routes.dib](Routes.dib) | Routes (modern) | Compute a route with traffic-aware preference and a field mask |
| [Directions.dib](Directions.dib) | Directions *(legacy)* | Turn-by-turn directions; legacy — prefer Routes |
| [DistanceMatrix.dib](DistanceMatrix.dib) | Distance Matrix *(legacy)* | Travel time/distance matrix; legacy — prefer Routes |
| [Elevation.dib](Elevation.dib) | Elevation | Elevation for points and along a path |
| [TimeZone.dib](TimeZone.dib) | Time Zone | Time zone and UTC/DST offsets for a location |
| [AddressValidation.dib](AddressValidation.dib) | Address Validation | Validate a postal address and read the verdict |
| [Roads.dib](Roads.dib) | Roads | Snap to roads, nearest roads, and speed limits |
| [StaticMaps.dib](StaticMaps.dib) | Static Maps | Build a map-image URL and render it inline |

### Billable ⚠️

These call paid APIs and **incur charges** on your Google Cloud project — each opens with a cost
warning. Run only if you accept the cost.

| Notebook | API surface | What it shows |
| --- | --- | --- |
| [PlacesNew.dib](PlacesNew.dib) | Places (New) | Text/nearby search, details, autocomplete, and photos |
| [Solar.dib](Solar.dib) | Solar | Building insights, data layers, and a GeoTIFF download |
| [AerialView.dib](AerialView.dib) | Aerial View | Render a flyover video and poll until it's ready |
