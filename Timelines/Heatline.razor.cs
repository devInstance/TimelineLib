using DevInstance.LogScope;
using DevInstance.Timelines.Model;
using DevInstance.Timelines.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInstance.Timelines
{
    public partial class Heatline
    {
        private TimeRange timeRange;
        private float cellWidthPercent;

        private HeatItem[] HeatItems;

        private int svgHeight;

        private IScopeLog log;

        protected override void OnInitialized()
        {
            log = ScopeManager.CreateLogger(this);
            using (var l = log.DebugExScope())
            {
                CalculateTimeRange();
                l.D($"timeRange.Span={timeRange.Span}");

                InitializeTimeScale();
                InitializeHeatlines();
            }
        }

        protected override void OnParametersSet()
        {
            using (var l = log.DebugExScope())
            {
                base.OnParametersSet();

                CalculateTimeRange();
                l.D($"timeRange.Span={timeRange.Span}");

                InitializeTimeScale();
                InitializeHeatlines();
                StateHasChanged();
            }
        }


        private void CalculateTimeRange()
        {
            using (var l = log.DebugExScope())
            {
                if (timeRange != null)
                {
                    l.DE($"Existing {timeRange.StartTime} - {timeRange.EndTime}");
                }
                l.DE($"Parent {Parent.Range.StartTime} - {Parent.Range.EndTime}");
                if (timeRange != Parent.Range)
                {
                    timeRange = Parent.Range;
                    cellWidthPercent = 100.0f / (float)timeRange.Span;

                    l.DE($"New range from parent {timeRange.StartTime} - {timeRange.EndTime}");

                    if (Parent.IsTimeRangeFlexible)
                    {
                        timeRange = TimeRangeCalculator.CalculateDynamicTimeRange(timeRange, TimeInterval, Data);
                    }

                    Parent.UpdateParentsTimeRange(timeRange);
                }
            }
        }

        private void InitializeTimeScale()
        {
            var now = DateTime.Today;
            using (var l = log.DebugExScope())
            {
                CalculateTimeRange();

                if (Data != null)
                {
                    svgHeight = Data.Count() * 36 + 10; //TODO: re-factor
                }
                else
                {
                    svgHeight = 46;
                }
                Parent.UpdateParentsHeight(svgHeight);
            }
        }

        private void InitializeHeatlines()
        {
            using (var l = log.DebugExScope())
            {
                var heatlines = new List<HeatItem>();

                if (Data != null)
                {
                    int n = 0;
                    foreach (var line in Data)
                    {
                        foreach (var it in line.Items)
                        {
                            float x = ((it.Time.Hour + (it.Time.Minute / 60.0f) - (float)timeRange.StartTime) * cellWidthPercent);
                            int y = n * 36 + 24;
                            var item = new HeatItem
                            {
                                CssClass = (line.CssClass != null ? line.CssClass.ToLower() : "white"),
                                X = $"{x}%",
                                Y = $"{y}",
                                Value = it.Value
                            };

                            heatlines.Add(item);
                        }

                        n++;
                    }
                }

                HeatItems = heatlines.ToArray();
            }
        }


        //protected override bool ShouldRender()
        //{
        //    using (var l = log.DebugExScope())
        //    {
        //        InitializeTimeScale();
        //        InitializeHeatlines();

        //        return true;
        //    }
        //}

    }
}
