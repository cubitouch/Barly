using System;
using System.Collections.Generic;
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
    }
}
