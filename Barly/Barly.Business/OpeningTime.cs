using System;
using System.Collections.Generic;

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
                if (Hours.Count > 1)
                    return string.Format("{0:HH}h{0:mm} - {1:HH}h{1:mm} / {2:HH}h{2:mm} - {3:HH}h{3:mm}", Hours[0].StartTime, Hours[0].EndTime, Hours[1].StartTime, Hours[1].EndTime);
                return string.Format("{0:HH}h{0:mm} - {1:HH}h{1:mm}", Hours[0].StartTime, Hours[0].EndTime);
            }
        }
    }
}
