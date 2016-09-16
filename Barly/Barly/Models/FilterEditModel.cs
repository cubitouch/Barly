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
        public IDictionary<double, double> Positions { get; set; }

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
            Positions = new Dictionary<double, double>();
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
            Positions = new Dictionary<double, double>();
            if (onlyOpenBars == "on")
                OnlyOpenBars = true;

            PositionA = new GeoCoordinate(lat, lng);

            Positions.Add(PositionA.Latitude, PositionA.Longitude);
        }
        public FilterEditModel(double latA, double lngA, double latB, double lngB, string onlyOpenBars)
        {
            Positions = new Dictionary<double, double>();
            if (onlyOpenBars == "on")
                OnlyOpenBars = true;

            PositionA = new GeoCoordinate(latA, lngA);
            Positions.Add(PositionA.Latitude, PositionA.Longitude);
            PositionB = new GeoCoordinate(latB, lngB);
            Positions.Add(PositionB.Latitude, PositionB.Longitude);
        }
    }
}
