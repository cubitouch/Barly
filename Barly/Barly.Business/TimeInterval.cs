using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barly.Business
{
    public class TimeInterval
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeInterval()
        {

        }
        public TimeInterval(string start, string end)
        {
            DateTime startTime;
            DateTime.TryParse(start, out startTime);
            StartTime = startTime;

            DateTime endTime;
            DateTime.TryParse(end, out endTime);
            if (endTime.Hour < 12)
                endTime = endTime.AddDays(1);
            EndTime = endTime;
        }
        public bool IsInInterval(DateTime time)
        {
            DateTime effectiveTime = DateTime.MinValue;
            DateTime.TryParse(time.ToString("HH:mm"), out effectiveTime);
            return (effectiveTime >= StartTime && effectiveTime <= EndTime);
        }
    }
}
