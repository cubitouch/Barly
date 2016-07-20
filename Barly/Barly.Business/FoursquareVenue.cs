using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CodeFluent.Runtime.Utilities;

namespace Barly.Business
{
    public class FoursquareVenue
    {
        private const string apiUrlFormat = "https://api.foursquare.com/v2/venues/{0}?client_id=OY20VC53QARVORPLBRGCD2SM0VDMQF33QYL1PWYFFG5LBZIZ&client_secret=XHAABUKDZHZRVJRCNUXLNOV1DHQIXAI5GJTSAADGW3UROVN1&v=20160718";

        public string Id { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Menu { get; set; }
        public int Price { get; set; }

        public FoursquareVenue(string urlOrId)
        {
            string id = "";
            if (urlOrId.StartsWith("http")) // parameter is URL
            {
                // example : https://fr.foursquare.com/v/abracadabar/4c77fcf0a8683704d6b40b4d
                id = urlOrId.Split('/').Last();
            }
            else // paremeter id ID
            {
                id = urlOrId;
            }

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                string json = client.DownloadString(string.Format(apiUrlFormat, id));

                var response = JsonUtilities.Deserialize<FoursquareAPIResponse>(json);
                //return response.venue;
                Id = response.venue.id;
                if (response.venue.contact.ContainsKey("phone"))
                    Phone = response.venue.contact["phone"];
                Website = response.venue.url;
                Menu = response.venue.menu.url;
                Price = response.venue.price.tier;
            }
        }
    }

    public class FoursquareAPIResponse
    {
        public FoursquareAPIVenue venue { get; set; }
    }
    public class FoursquareAPIVenue
    {
        public string id { get; set; }
        public Dictionary<string, string> contact { get; set; }
        public string url { get; set; }
        public FoursquareAPIMenu menu { get; set; }
        public FoursquareAPIPrice price { get; set; }
    }
    public class FoursquareAPIPrice
    {
        public int tier { get; set; }
    }
    public class FoursquareAPIMenu
    {
        public string url { get; set; }
    }
}
