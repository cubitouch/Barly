using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using CodeFluent.Runtime.Utilities;

namespace Barly.Business
{
    public class GoogleNearby
    {
        private const string apiUrlFormat = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?key=AIzaSyD61F5jFB_8ql02dWz6ql73Ve076nTEuQE&location={0},{1}&types=subway_station&radius=500";

        public List<GoogleNearbyStation> Subways { get; set; }

        public GoogleNearby()
        {
            Subways = new List<GoogleNearbyStation>();
        }

        public GoogleNearby(double lat, double lng) : this()
        {
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                string json = client.DownloadString(string.Format(apiUrlFormat, lat, lng));

                var baseResponse = JsonUtilities.Deserialize<GoogleNearbyAPIBase>(json);

                if (baseResponse.results != null)
                {
                    List<SubwayStation> stations = JsonUtilities.Deserialize<List<SubwayStation>>(System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/App_Data/ratp.json")));
                    foreach (GoogleNearbyAPIResult result in baseResponse.results)
                    {
                        var station = stations.First(s => s.station == result.name || s.alias == result.name);
                        if (Subways.Count(s => s.name == station.station) == 0)
                        {
                            var nearbyStation = new GoogleNearbyStation();
                            nearbyStation.name = station.station;
                            foreach (string line in station.lines)
                            {
                                nearbyStation.lines.Add(line);
                            }
                            Subways.Add(nearbyStation);
                        }
                    }
                }
            }
        }
    }

    public class GoogleNearbyStation
    {
        public string name { get; set; }
        public List<string> lines { get; set; }

        public GoogleNearbyStation()
        {
            lines = new List<string>();
        }
    }

    public class GoogleNearbyAPIBase
    {
        public List<GoogleNearbyAPIResult> results { get; set; }

        public GoogleNearbyAPIBase()
        {
            //results = new GoogleNearbyAPIResult();
        }
    }

    public class GoogleNearbyAPIResult
    {
        public string name { get; set; }
    }

    public class SubwayStation
    {
        public string station { get; set; }
        public string alias { get; set; }
        public List<string> lines { get; set; }

        public SubwayStation()
        {

        }
    }
}
