using DevInstance.LogScope;
using DevInstance.Timelines.Model;
using DevInstance.Timelines.Utils;
using System;
using System.Collections.Generic;

namespace DevInstance.Timelines
{
    public partial class Chart : ITimelineChart
    {
        private DateTime currentDateTime = DateTime.Now;

        private TimeRange timeRange;
        private float cellWidthPercent;

        private TimeScaleLabelItem[] TimeScaleLabel;
        private TimeScaleItem[] TimeScale;

        private IScopeLog log;

        TimeRange ITimelineChart.Range => timeRange;

        bool ITimelineChart.IsTimeRangeFlexible => IsTimeRangeFlexible;

        DateTime ITimelineChart.CurrentDateTime => CurrentDateTime;

        protected override void OnInitialized()
        {
            log = ScopeManager.CreateLogger(this);
            using (var l = log.DebugExScope())
            {
                timeRange = TimeRangeCalculator.CreateTimeRange(StartTime, EndTime);

                if(Lines == null)
                {
                    // if no child template then render scale for the default range
                    RenderTimeScale();
                }
                //StateHasChanged();
            }
        }

        protected override void OnParametersSet()
        {
            using (var l = log.DebugExScope())
            {
                base.OnParametersSet();

                l.DE($"Parameters {StartTime} - {EndTime}");
                var r = TimeRangeCalculator.CreateTimeRange(StartTime, EndTime);

                if(r != timeRange)
                {
                    timeRange = r;
                    l.DE($"New time range");
                    RenderTimeScale();
                    StateHasChanged();
                }
            }
        }

        private void RenderTimeScale()
        {
            var now = DateTime.Today;
            using (var l = log.DebugExScope())
            {
                cellWidthPercent = 100.0f / (float)timeRange.Span;

                var labels = new List<TimeScaleLabelItem>();
                for (int time = (int)timeRange.StartTime; time <= timeRange.EndTime; time += 1)
                {
                    var item = new TimeScaleLabelItem();

                    item.CssClass = time % 2 == 1 ? "odd-label" : "even-label";
                    item.LabelText = now.AddHours(time).ToString("hhtt").ToLower(); //TODO: needs i18n review

                    if (time == timeRange.StartTime || time == timeRange.EndTime - 1)
                    {
                        item.DivStyle = $"max-width:{cellWidthPercent - 1}%;width:{cellWidthPercent - 1}%";
                    }
                    else if (time == timeRange.EndTime)
                    {
                        item.DivStyle = $"max-width:1%;width:1%";
                    }
                    else
                    {
                        item.DivStyle = $"max-width:{cellWidthPercent}%;width:{cellWidthPercent}%";
                    }
                    labels.Add(item);
                }
                TimeScaleLabel = labels.ToArray();

                var items = new List<TimeScaleItem>();
                for (int i = 0, count = (int)timeRange.Span; i < count; i++)
                {
                    var item = new TimeScaleItem
                    {
                        HourLineX = $"{i * cellWidthPercent}%",
                        HourLineY = DefaultValues.HourLineLength,
                        HalfLineX = $"{i * cellWidthPercent + cellWidthPercent / 2}%",
                        HalfLineY = DefaultValues.HalfHourLineLength
                    };
                    items.Add(item);
                }

                TimeScale = items.ToArray();
            }
        }

        protected override bool ShouldRender()
        {
            using (var l = log.DebugExScope())
            {
                //InitializeTimeScale();
                var res = base.ShouldRender();
                l.DE($"result = ${res}");
                return res;
            }
        }

        void ITimelineChart.UpdateParentsHeight(int value)
        {
            using (var l = log.DebugExScope())
            {
                var newValue = value.ToString();
                if (Height != newValue)
                {
                    Height = newValue;
                    StateHasChanged();
                }
            }
        }

        void ITimelineChart.UpdateParentsTimeRange(TimeRange r)
        {
            using (var l = log.DebugExScope())
            {
                if (TimeScaleLabel == null || timeRange != r)
                {
                    timeRange = r;
                    RenderTimeScale();
                    StateHasChanged();
                }
            }
        }
    }
}
