using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInstance.TimelineLib.Utils
{
    public class TimeRange
    {
        public TimeRange(double startTime, double endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public double Span => EndTime - StartTime;

        public double StartTime { get; internal set; }
        public double EndTime { get; internal set; }
    }
}
