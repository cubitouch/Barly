using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RowShare.Api;

namespace Barly.Business
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<OpeningTime> OpeningTimes { get; set; }
        public string Picture { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsValid { get; set; }
        public bool IsOpenNow
        {
            get
            {
                var now = DateTime.Now;
                OpeningTime schedule = OpeningTimes.FirstOrDefault(ot => ot.DayOfWeek == now.DayOfWeek);

                // bar is closed
                if (schedule == null)
                    return false;

                // bar is open
                if (schedule.Hours.Count(h => h.IsInInterval(now)) > 0)
                    return true;

                // bar is not open yet
                return false;
            }
        }

        public Location()
        {

        }

        private static string GetResourceLink(Row row, string key)
        {
            if (row == null)
                return string.Empty;

            File file = new File((IDictionary<string, object>)row.Values["Photo"]);
            return string.Format(CultureInfo.CurrentCulture, "https://www.rowshare.com/blob/{0}4/1/{1}", row.Id.ToString().Replace("-", ""), file.FileName);
        }

        private OpeningTime ExtractOpeningTime(Row row, DayOfWeek dayOfWeek, string day, bool isToday)
        {
            string dayValue = string.Concat(row.Values[day]);
            var result = new OpeningTime()
            {
                DayOfWeek = dayOfWeek,
                DayOfWeekMessage = day,
                IsOpen = !string.IsNullOrWhiteSpace(dayValue),
                IsToday = isToday,
                Hours = ExtractHours(dayValue)
            };

            return result;
        }
        public Location(Row row)
        {
            Id = int.Parse(row.Values["Id"].ToString());
            Name = row.Values["Nom"].ToString();
            Description = row.Values["Description"].ToString();
            Picture = GetResourceLink(row, "Photo");
            Address = row.Values["Adresse"].ToString();
            ZipCode = row.Values["Code postal"].ToString();
            Latitude = double.Parse(row.Values["Latitude"].ToString());
            Longitude = double.Parse(row.Values["Longitude"].ToString());
            IsValid = bool.Parse(row.Values["Validé"].ToString());

            OpeningTimes = new List<OpeningTime>();
            OpeningTimes.Add(ExtractOpeningTime(row, DayOfWeek.Monday, "Lundi", DateTime.Today.DayOfWeek == DayOfWeek.Monday));
            OpeningTimes.Add(ExtractOpeningTime(row, DayOfWeek.Tuesday, "Mardi", DateTime.Today.DayOfWeek == DayOfWeek.Tuesday));
            OpeningTimes.Add(ExtractOpeningTime(row, DayOfWeek.Wednesday, "Mercredi", DateTime.Today.DayOfWeek == DayOfWeek.Wednesday));
            OpeningTimes.Add(ExtractOpeningTime(row, DayOfWeek.Monday, "Jeudi", DateTime.Today.DayOfWeek == DayOfWeek.Thursday));
            OpeningTimes.Add(ExtractOpeningTime(row, DayOfWeek.Friday, "Vendredi", DateTime.Today.DayOfWeek == DayOfWeek.Friday));
            OpeningTimes.Add(ExtractOpeningTime(row, DayOfWeek.Saturday, "Samedi", DateTime.Today.DayOfWeek == DayOfWeek.Saturday));
            OpeningTimes.Add(ExtractOpeningTime(row, DayOfWeek.Sunday, "Dimanche", DateTime.Today.DayOfWeek == DayOfWeek.Sunday));
        }

        // 1 service => XX:XX - XX:XX
        // 2 services => XX:XX - XX:XX / XX:XX - XX:XX
        private List<TimeInterval> ExtractHours(string hours)
        {
            var intervals = new List<TimeInterval>();

            string formatedHours = hours.Replace(" ", "");
            string[] services = formatedHours.Split('/');

            foreach (string service in services)
            {
                var times = service.Split('-');
                if (times.Length == 2)
                    intervals.Add(new TimeInterval(times[0], times[1]));
            }

            return intervals;
        }
    }
}
