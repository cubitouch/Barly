using System;
using System.Collections.Generic;
using System.Device.Location;
using Barly.Business;

namespace Barly.Models
{
    public class FilterEditModel
    {
        public IList<string> ZipCodes { get; set; }

        public FilterEditModel()
        {

        }

        public FilterEditModel(IList<string> zipcodes)
        {
            ZipCodes = new List<string>();

            if (zipcodes!= null)
            {
                foreach (string zipcode in zipcodes)
                {
                    ZipCodes.Add(zipcode);
                }
            }
        }
    }
}
