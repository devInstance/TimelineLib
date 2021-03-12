using DevInstance.Timelines.Model;
using DevInstance.Timelines.Utils;
using System;

namespace DevInstance.Timelines
{
    interface ITimelineChart
    {
        TimeRange Range { get; }
        bool IsTimeRangeFlexible { get; }
        DateTime CurrentDateTime { get; }
        void UpdateParentsHeight(int value);
        void UpdateParentsTimeRange(TimeRange timeRange);
        void SetLineLabels(TimeScaleLabelItem[] labels);
    }
}
