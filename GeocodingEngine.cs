using GoogleMapsApi;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;
using GoogleMapsApi.Entities.Geocoding.Request;
using GoogleMapsApi.Entities.Geocoding.Response;
using GoogleMapsApi.StaticMaps;
using GoogleMapsApi.StaticMaps.Entities;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Upswing
{
    public class GeocodingEngine
    {
        public string GetStaticMapUrl(string origin, string destination)
        {
            DirectionsRequest directionsRequest = new DirectionsRequest()
            {
                ApiKey = ConfigurationManager.AppSettings["ApiKey"],
                Origin = origin,
                Destination = destination,
            };

            DirectionsResponse directions = GoogleMaps.Directions.Query(directionsRequest);

            // Static maps API - get static map of with the path of the directions request
            StaticMapsEngine staticMapGenerator = new StaticMapsEngine();

            //Path from previos directions request
            IEnumerable<Step> steps = directions.Routes.First().Legs.First().Steps;
            // All start locations
            IList<ILocationString> path = steps.Select(step => step.StartLocation).ToList<ILocationString>();
            // also the end location of the last step
            path.Add(steps.Last().EndLocation);

            var startingPoint = steps.First().EndLocation;

            var staticMapRequest = new StaticMapRequest(new Location(startingPoint.Latitude, startingPoint.Longitude), 9, new ImageSize(650, 500))
            {
                ApiKey = ConfigurationManager.AppSettings["ApiKey"]
            };

            return staticMapGenerator.GenerateStaticMapURL(staticMapRequest);
        }

        public string GetStaticMapUrl(string address)
        {
            GeocodingRequest geocodeRequest = new GeocodingRequest()
            {
                Address = address,
                ApiKey = ConfigurationManager.AppSettings["ApiKey"]
            };
            var geocodingEngine = GoogleMaps.Geocode;
            GeocodingResponse geocode = geocodingEngine.Query(geocodeRequest);

            var firstResult = geocode.Results.First();
      //html pour l'utilisation de l'API 
            return $@"
<!DOCTYPE html>
<html>
<head>
<meta http-equiv=""X-UA-Compatible"" content=""IE=Edge"" />
</head>
<body>
<iframe 
width=""600"" 
height = ""450"" 
style = ""border:0"" 
loading = ""lazy""
src = ""https://www.google.com/maps/embed/v1/place?key={ConfigurationManager.AppSettings["ApiKey"]}&q=place_id:{firstResult.PlaceId}"">
</iframe > 
</body>
</html>
";

        }
    }
}