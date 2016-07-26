using System;
using System.Collections.Generic;
using System.Device.Location;
using Barly.Business;

namespace Barly.Models
{
    public class SearchResultModel
    {
        public IList<Location> Locations { get; set; }
        public IDictionary<double, double> Positions { get; set; }
        public IList<string> Filters { get; set; }

        public SearchResultModel()
        {
            Locations = new List<Location>();
            Positions = new Dictionary<double, double>();
            Filters = new List<string>();
        }

        public SearchResultModel(IList<string> zipcodes) : this()
        {
            var backOffice = new BackOffice();

            foreach (Location location in backOffice.Locations)
            {
                if (zipcodes.Contains(location.ZipCode) && location.IsValid)
                {
                    Locations.Add(location);
                }
            }
            
            foreach (string zipcode in zipcodes)
            {
                Filters.Add(zipcode);
            }
        }


        public SearchResultModel(double latitude, double longitude) : this()
        {
            var backOffice = new Business.BackOffice();

            int zone = 0;
            while (Locations.Count < findNumberBarFromLocations)
            {
                zone += 200;
                foreach (Location location in backOffice.Locations)
                {
                    var searchCoordinate = new GeoCoordinate(latitude, longitude);
                    var locationCoordinate = new GeoCoordinate(location.Latitude, location.Longitude);
                    if (searchCoordinate.GetDistanceTo(locationCoordinate) < zone && location.IsValid && !Locations.Contains(location))
                    {
                        Locations.Add(location);
                    }
                }
            }
        }

        private const int findNumberBarFromLocations = 5;

        private static double DegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        private static double RadiansToDegrees(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
        private GeoCoordinate MidPoint(GeoCoordinate posA, GeoCoordinate posB)
        {
            GeoCoordinate midPoint = new GeoCoordinate();

            double dLon = DegreesToRadians(posB.Longitude - posA.Longitude);
            double Bx = Math.Cos(DegreesToRadians(posB.Latitude)) * Math.Cos(dLon);
            double By = Math.Cos(DegreesToRadians(posB.Latitude)) * Math.Sin(dLon);

            midPoint.Latitude = RadiansToDegrees(Math.Atan2(
                         Math.Sin(DegreesToRadians(posA.Latitude)) + Math.Sin(DegreesToRadians(posB.Latitude)),
                         Math.Sqrt(
                             (Math.Cos(DegreesToRadians(posA.Latitude)) + Bx) *
                             (Math.Cos(DegreesToRadians(posA.Latitude)) + Bx) + By * By)));
            // (Math.Cos(DegreesToRadians(posA.Latitude))) + Bx) + By * By)); // Your Code

            midPoint.Longitude = posA.Longitude + RadiansToDegrees(Math.Atan2(By, Math.Cos(DegreesToRadians(posA.Latitude)) + Bx));

            return midPoint;
        }
        public SearchResultModel(double latitudeA, double longitudeA, double latitudeB, double longitudeB) : this()
        {
            var backOffice = new Business.BackOffice();
            var locationCoordinateA = new GeoCoordinate(latitudeA, longitudeA);
            var locationCoordinateB = new GeoCoordinate(latitudeB, longitudeB);
            var searchCoordinate = MidPoint(locationCoordinateA, locationCoordinateB);
            Positions.Add(locationCoordinateA.Latitude, locationCoordinateA.Longitude);
            Positions.Add(locationCoordinateB.Latitude, locationCoordinateB.Longitude);

            int zone = 0;
            while (Locations.Count < findNumberBarFromLocations)
            {
                zone += 200;
                foreach (Location location in backOffice.Locations)
                {
                    var locationCoordinate = new GeoCoordinate(location.Latitude, location.Longitude);
                    if (searchCoordinate.GetDistanceTo(locationCoordinate) < zone && location.IsValid && !Locations.Contains(location))
                    {
                        Locations.Add(location);
                    }

                }
            }
        }

        public SearchResultModel(int id) : this()
        {
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
