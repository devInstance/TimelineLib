using DevInstance.Timelines.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInstance.Timelines
{
    interface ITimelineChart
    {
        TimeRange Range { get; }
        bool IsTimeRangeFlexible { get; }
        DateTime CurrentDateTime { get; }
        void UpdateParentsHeight(int value);
        void UpdateParentsTimeRange(TimeRange timeRange);
    }
}
