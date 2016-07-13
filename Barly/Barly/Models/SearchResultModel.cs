using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Barly.Business;

namespace Barly.Models
{
    public class SearchResultModel
    {
        public IList<Location> Locations { get; set; }
        public IList<string> Filters { get; set; }

        public SearchResultModel()
        {

        }

        public SearchResultModel(IList<string> zipcodes)
        {
            Locations = new List<Location>();

            var backOffice = new Business.BackOffice();

            foreach (Location location in backOffice.Locations)
            {
                if (zipcodes.Contains(location.ZipCode) && location.IsValid)
                {
                    Locations.Add(location);
                }
            }

            Filters = new List<string>();
            foreach (string zipcode in zipcodes)
            {
                Filters.Add(zipcode);
            }
        }


        public SearchResultModel(double latitude, double longitude)
        {
            Locations = new List<Location>();

            var backOffice = new Business.BackOffice();

            foreach (Location location in backOffice.Locations)
            {
                var searchCoordinate = new GeoCoordinate(latitude, longitude);
                var locationCoordinate = new GeoCoordinate(location.Latitude, location.Longitude);
                if (searchCoordinate.GetDistanceTo(locationCoordinate) < 2000 && location.IsValid)
                {
                    Locations.Add(location);
                }
            }

            Filters = new List<string>();
        }

        public SearchResultModel(int id)
        {
            Locations = new List<Location>();
            Filters = new List<string>();

            var backOffice = new Business.BackOffice();

            foreach (Location location in backOffice.Locations)
            {
                if (location.Id == id && location.IsValid)
                {
                    Locations.Add(location);
                    Filters.Add(location.Name);
                }
            }

        }
    }
}
