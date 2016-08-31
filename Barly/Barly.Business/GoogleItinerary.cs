using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CodeFluent.Runtime.Utilities;

namespace Barly.Business
{
    public class GoogleItinerary
    {
        private const string apiUrlFormat = "https://maps.googleapis.com/maps/api/directions/json?key=AIzaSyD61F5jFB_8ql02dWz6ql73Ve076nTEuQE&origin={0},{1}&destination={2},{3}&mode=transit";


        public GoogleItinerary()
        {
        }

        public GoogleItineraryBase GetItinerary(GeoCoordinate positionA, GeoCoordinate positionB)
        {
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                string json = client.DownloadString(string.Format(apiUrlFormat, positionA.Latitude, positionA.Longitude, positionB.Latitude, positionB.Longitude));

                var baseResponse = JsonUtilities.Deserialize<GoogleItineraryBase>(json);
                return baseResponse;
            }
        }
    }

    public class GoogleItineraryBase
    {
        public List<GoogleItineraryWayPoint> geocoded_waypoints { get; set; }
        public List<GoogleItineraryRoute> routes { get; set; }

        public GoogleItineraryBase()
        {
            geocoded_waypoints = new List<GoogleItineraryWayPoint>();
            routes = new List<GoogleItineraryRoute>();
        }
    }

    public class GoogleItineraryWayPoint
    {
        public string geocoder_status { get; set; }
        public string place_id { get; set; }
        public string types { get; set; }
    }

    public class GoogleItineraryRoute
    {
        public List<GoogleItineraryLeg> legs { get; set; }
    }

    public class GoogleItineraryLeg
    {
        public Dictionary<string, double> start_location { get; set; }
        public List<GoogleItineraryStep> steps { get; set; }
    }

    public class GoogleItineraryStep
    {
        public Dictionary<string, string> duration { get; set; }
        public Dictionary<string, double> end_location { get; set; }
        public Dictionary<string, string> polyline { get; set; }
    }
}
