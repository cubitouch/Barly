using System;
using System.Collections.Generic;
using System.Linq;

namespace Barly.Business
{
    public class OpeningTime
    {
        public DayOfWeek DayOfWeek { get; set; }
        public string DayOfWeekMessage { get; set; }
        public bool IsOpen { get; set; }
        public bool IsToday { get; set; }
        public List<TimeInterval> Hours { get; set; }
        public string HoursFormat
        {
            get
            {
                if (Hours.Count == 0)
                    return "Fermé";
                string format = "";
                foreach (TimeInterval hour in Hours)
                {
                    format += string.Format("{0:HH}h{0:mm} - {1:HH}h{1:mm}", hour.StartTime, hour.EndTime);
                    if (hour != Hours.Last())
                        format += " / ";
                }
                return format;

            }
        }
    }
}
