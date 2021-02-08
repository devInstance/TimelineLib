using DevInstance.Timelines.Model;
using System.Collections.Generic;

namespace DevInstance.Timelines.Utils
{
    public static class TimeRangeCalculator
    {
        public static TimeRange CreateTimeRange(double? start, double? end)
        {
            return new TimeRange(Normilize(start, DefaultValues.START_TIME),
                                    Normilize(end, DefaultValues.END_TIME));
        }

        private static double Normilize(double? value, double defaultValue)
        {
            if (!value.HasValue) return defaultValue;

            if (value.Value < 0.0)
            {
                return 0.0;
            }
            else if (value.Value > 24.0)
            {
                return 24.0;
            }
            return value.Value;
        }

        public static TimeRange CalculateDynamicTimeRange(TimeRange inRange, IEnumerable<Timeline.Line> data)
        {
            var isMinUpdated = false;
            var isMaxUpdated = false;

            var minStart = inRange.StartTime;
            var maxEnd = inRange.EndTime;

            foreach (var line in data)
            {
                foreach (var item in line.Items)
                {
                    if (item.StartTime.TimeOfDay.TotalHours < minStart)
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

        public static TimeRange CalculateDynamicTimeRange(TimeRange inRange, double timeInterval, IEnumerable<Heatline.Line> data)
        {
            var isMinUpdated = false;
            var isMaxUpdated = false;

            var minStart = inRange.StartTime;
            var maxEnd = inRange.EndTime;

            foreach (var line in data)
            {
                foreach(var item in line.Items)
                {
                    if(item.Time.TimeOfDay.TotalHours < minStart)
                    {
                        isMinUpdated = true;
                        minStart = item.Time.TimeOfDay.TotalHours;
                    }

                    if (item.Time.TimeOfDay.TotalHours > maxEnd)
                    {
                        isMaxUpdated = true;
                        maxEnd = item.Time.TimeOfDay.TotalHours;
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
                maxEnd = (int)maxEnd + timeInterval;
            }

            return new TimeRange(minStart, maxEnd);
        }
    }
}
