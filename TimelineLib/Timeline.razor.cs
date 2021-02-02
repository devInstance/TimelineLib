using DevInstance.LogScope;
using DevInstance.TimelineLib.Model;
using DevInstance.TimelineLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevInstance.TimelineLib
{
    public partial class Timeline
    {
        private DateTime currentDateTime = DateTime.Now;

        private TimeRange timeRange;
        private float cellWidthPercent;

        private TimeBar[] TimeBars;

        private int svgHeight;

        private IScopeLog log;

        protected override void OnInitialized()
        {
            log = ScopeManager.CreateLogger(this);
            using (var l = log.DebugExScope())
            {
                CalculateTimeRange();

                cellWidthPercent = 100.0f / (float)timeRange.Span;

                InitializeTimeScale();
                InitializeTimeBars();
                StateHasChanged();
            }
        }

        private void CalculateTimeRange()
        {
            timeRange = TimeRangeCalculator.CreateTimeRange(StartTime, EndTime);

            if (IsTimeRangeFlexible)
            {
                timeRange = TimeRangeCalculator.CalculateDynamicTimeRange(timeRange, Data);
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
                    svgHeight = Data.Count() * 36 + 10; //TODO: refactor
                }
                else
                {
                    svgHeight = 46;
                }
            }
        }

        private void InitializeTimeBars()
        {
            var bars = new List<TimeBar>();

            if (Data != null)
            {
                int n = 0;
                foreach (var task in Data)
                {
                    foreach (var tl in task.Items)
                    {
                        float x = ((tl.StartTime.Hour + (tl.StartTime.Minute / 60.0f) - (float)timeRange.StartTime) * cellWidthPercent);
                        int y = n * 36 + 10;
                        float width = ((float)tl.ElapsedTime.TotalMinutes / 60.0f * cellWidthPercent);
                        int height = 26;
                        var item = new TimeBar
                        {
                            CssClass = "bar " + (task.CssClass != null ? task.CssClass.ToLower() : "white"),
                            Left = $"{x}%",
                            Top = $"{y}",
                            Width = $"{width}%",
                            Height = $"{height}",
                            LabelX = $"{x + 0.25}%",
                            LabelY = $"{y + 18}"
                        };

                        var tm = tl.ElapsedTime;
                        //TODO: i18n review
                        var timeFormat = tm.Hours > 0 ? "{0}: {1:hh} hours and {1:%m} minutes" : "{0}: {1:%m} minutes";
                        item.Tooltip = String.Format(timeFormat, tl.Title, tm);

                        bars.Add(item);
                    }

                    n++;
                }
            }

            TimeBars = bars.ToArray();
        }


        protected override bool ShouldRender()
        {
            using (var l = log.DebugExScope())
            {
                InitializeTimeScale();
                InitializeTimeBars();

                return true;
            }
        }

    }
}
