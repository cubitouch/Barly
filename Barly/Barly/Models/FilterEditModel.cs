using System;
using System.Collections.Generic;
using System.Device.Location;
using Barly.Business;

namespace Barly.Models
{
    public enum FilterMode
    {
        Default,
        Geolocation,
        Midway
    }
    public class FilterEditModel
    {
        public bool OnlyOpenBars { get; set; }
        public IList<string> ZipCodes { get; set; }
        public GeoCoordinate PositionA { get; set; }
        public GeoCoordinate PositionB { get; set; }

        public FilterMode Mode
        {
            get
            {
                if (PositionB != null)
                    return FilterMode.Midway;
                if (PositionA != null)
                    return FilterMode.Geolocation;
                return FilterMode.Default;
            }
        }

        public FilterEditModel()
        {

        }

        public FilterEditModel(IList<string> zipcodes, string onlyOpenBars)
        {
            if (onlyOpenBars == "on")
                OnlyOpenBars = true;

            ZipCodes = new List<string>();

            if (zipcodes != null)
            {
                foreach (string zipcode in zipcodes)
                {
                    ZipCodes.Add(zipcode);
                }
            }
        }
        public FilterEditModel(double lat, double lng, string onlyOpenBars)
        {
            if (onlyOpenBars == "on")
                OnlyOpenBars = true;

            PositionA = new GeoCoordinate(lat, lng);
        }
        public FilterEditModel(double latA, double lngA, double latB, double lngB, string onlyOpenBars)
        {
            if (onlyOpenBars == "on")
                OnlyOpenBars = true;

            PositionA = new GeoCoordinate(latA, lngA);
            PositionB = new GeoCoordinate(latB, lngB);
        }
    }
}
