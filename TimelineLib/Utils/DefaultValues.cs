using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInstance.TimelineLib.Utils
{
    public static class DefaultValues
    {
        /// <summary>
        /// Default start time of the chart in hours.
        /// </summary>
        public static readonly double START_TIME = 8.0;
        public static readonly double END_TIME = 20.0;
        /// <summary>
        /// Margin time in minutes. Margin time is a time between
        /// min start time and max end time of the item.
        /// </summary>
        public static readonly double TIMERANGE_MARGIN = 30.0;
    }
}
