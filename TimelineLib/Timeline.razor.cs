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

        private ILog log;

        protected override void OnInitialized()
        {
            log = LogProvider.CreateLogger(this);
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
                            cssClass = "bar " + (task.CssClass != null ? task.CssClass.ToLower() : "white"),
                            x = $"{x}%",
                            y = $"{y}",
                            width = $"{width}%",
                            height = $"{height}",
                            labelx = $"{x + 0.25}%",
                            labely = $"{y + 18}"
                        };

                        if (width > 3.0)
                        {
                            //if (task.IsRunning && task.ActiveTimeLogItem.Id == tl.Id)
                            //{
                            //    item.label = String.Format("{0:F1} hrs", task.GetTotalHoursSpentTodayTillNow(TimeProvider));
                            //}
                            //else
                            //{
                            // item.label = String.Format("{0:F1}", tl.GetElapsedThisPeriodHours(TimeProvider));
                            //}
                        }
                        item.labeltooltip = $"{tl.StartTime.ToShortTimeString()} {tl.ElapsedTime.ToString()}";

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
