using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInstance.Timelines.Model
{
    public static class DefaultValues
    {
        /// <summary>
        /// Default start time of the chart in hours.
        /// </summary>
        public static readonly double START_TIME = 8.0;
        /// <summary>
        /// Default end time of the chart in hours.
        /// </summary>
        public static readonly double END_TIME = 20.0;
        /// <summary>
        /// Margin time in minutes. Margin time is a time between
        /// min start time and max end time of the item.
        /// </summary>
        public static readonly double TIMERANGE_MARGIN = 30.0;
        /// <summary>
        /// Default length of hour line in the time scale
        /// </summary>
        public static readonly string HourLineLength = "16";
        /// <summary>
        /// Default length of half-hour line in the time scale (= HourLineLength / 2)
        /// </summary>
        public static readonly string HalfHourLineLength = "8";
    }
}
