using DevInstance.TimelineLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DevInstance.TimelineLib.Timeline;

namespace DevInstance.TimelineLib.Utils
{
    public static class TimeRangeCalculator
    {
        public static TimeRange CreateTimeRange(double? start, double? end)
        {
            return new TimeRange(start.HasValue ? start.Value : DefaultValues.START_TIME,
                                    end.HasValue ? end.Value : DefaultValues.END_TIME);
        }

        public static TimeRange CalculateDynamicTimeRange(TimeRange inRange, IEnumerable<Line> data)
        {
            var isMinUpdated = false;
            var isMaxUpdated = false;

            var minStart = inRange.StartTime;
            var maxEnd = inRange.EndTime;

            foreach (var line in data)
            {
                foreach(var item in line.Items)
                {
                    if(item.StartTime.TimeOfDay.TotalHours < minStart)
                    {
                        isMinUpdated = true;
                        minStart = item.StartTime.TimeOfDay.TotalHours;
                    }

                    if (item.EndTime.TimeOfDay.TotalHours > maxEnd)
                    {
                        isMaxUpdated = true;
                        maxEnd = item.EndTime.TimeOfDay.TotalHours;
                    }
                }
            }

            //Add margins
            if (isMinUpdated)
            {
                minStart = (int)minStart;
            }
            if (isMaxUpdated)
            {
                maxEnd = (int)maxEnd + 1;
            }

            return new TimeRange(minStart, maxEnd);
        }
    }
}
