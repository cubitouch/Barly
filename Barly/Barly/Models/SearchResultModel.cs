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

        private const int _findNumberBarFromLocations = 5;

        public SearchResultModel()
        {
            Locations = new List<Location>();
            Positions = new Dictionary<double, double>();
        }

        public SearchResultModel(FilterEditModel filters) : this()
        {
            switch (filters.Mode)
            {
                case FilterMode.Geolocation:
                    {
                        SearchForGeolocation(filters);
                        break;
                    }
                case FilterMode.Midway:
                    {
                        SearchForMidway(filters);
                        break;
                    }
                default:
                    {
                        SearchForDefault(filters);
                        break;
                    }
            }
        }

        private void SearchForDefault(FilterEditModel filters)
        {
            var backOffice = new BackOffice();

            foreach (Location location in backOffice.Locations)
            {
                if (filters.ZipCodes.Contains(location.ZipCode) && location.IsValid && (!filters.OnlyOpenBars || location.IsOpenNow))
                {
                    Locations.Add(location);
                }
            }
        }

        private void SearchForGeolocation(FilterEditModel filters)
        {
            InitLocationsByCoordinate(filters.PositionA, filters.OnlyOpenBars);
            Positions.Add(filters.PositionA.Latitude, filters.PositionA.Longitude);
        }

        private void SearchForMidway(FilterEditModel filters)
        {
            GeoCoordinate searchCoordinate = MidPoint(filters.PositionA, filters.PositionB);
            Positions.Add(filters.PositionA.Latitude, filters.PositionA.Longitude);
            Positions.Add(filters.PositionB.Latitude, filters.PositionB.Longitude);

            InitLocationsByCoordinate(searchCoordinate, filters.OnlyOpenBars);
        }

        private void InitLocationsByCoordinate(GeoCoordinate coordinate, bool onlyOpen)
        {
            var backOffice = new Business.BackOffice();

            int zone = 0;
            int notMatchableBarCount = 0;
            while (Locations.Count < _findNumberBarFromLocations && notMatchableBarCount < backOffice.Locations.Count)
            {
                notMatchableBarCount = 0;
                zone += 200;
                foreach (Location location in backOffice.Locations)
                {
                    var locationCoordinate = new GeoCoordinate(location.Latitude, location.Longitude);
                    if (location.IsValid && !Locations.Contains(location) && (!onlyOpen || onlyOpen && location.IsOpenNow))
                    {
                        if (coordinate.GetDistanceTo(locationCoordinate) < zone)
                        {
                            Locations.Add(location);
                        }
                    }
                    else
                    {
                        notMatchableBarCount++;
                    }
                }
            }
        }

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

        public SearchResultModel(int id) : this()
        {
            var backOffice = new Business.BackOffice();

            foreach (Location location in backOffice.Locations)
            {
                if (location.Id == id && location.IsValid)
                {
                    Locations.Add(location);
                }
            }

        }
    }
}
