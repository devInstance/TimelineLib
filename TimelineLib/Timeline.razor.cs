using DevInstance.LogScope;
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
        struct TimeScaleLabelItem
        {
            public string cssClass;
            public string divStyle;
            public string labelText;
        }

        struct TimeScaleItem
        {
            public string lineonex;
            public string lineoney;

            public string linetwox;
            public string linetwoy;
        }

        struct TimeBar
        {
            public string cssClass;
            public string x;
            public string y;
            public string width;
            public string height;
            public string label;
            public string labelx;
            public string labely;
            public string labeltooltip;
        }

        private ILogProvider LogProvider = new ConsoleLogProvider(LogLevel.DEBUG_EXTRA); //TODO: refactor

        private DateTime currentDateTime = DateTime.Now;

        //private DateTime startDate;
        //private DateTime endDate;
        private TimeRange timeRange;
        private float cellWidthPercent;

        private TimeScaleLabelItem[] TimeScaleLabel;
        private TimeScaleItem[] TimeScale;
        private TimeBar[] TimeBars;

        private int svgHeight;

        private ILog log;

        protected override void OnInitialized()
        {
            log = LogProvider.CreateLogger(this);
            using (var l = log.DebugExScope())
            {
                //    DateTime now = currentDateTime;

                //    startDate = DateTime.Today;

                //    if (now.Hour < 8)
                //    {
                //        if (now.Hour > 1)
                //        {
                //            startDate = DateTime.Today.AddHours(now.Hour - 1);
                //        }
                //    }
                //    else
                //    {
                //        startDate = DateTime.Today.AddHours(8);
                //    }

                //    if (now.Hour >= 20)
                //    {
                //        if (now.Hour < 23)
                //        {
                //            endDate = DateTime.Today.AddHours(now.Hour + 1);
                //        }
                //        else
                //        {
                //            endDate = DateTime.Today.AddHours(now.Hour);
                //        }
                //    }
                //    else
                //    {
                //        endDate = DateTime.Today.AddHours(20);
                //    }

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

                var labels = new List<TimeScaleLabelItem>();
                for (int time = (int)timeRange.StartTime; time <= timeRange.EndTime; time += 1)
                {
                    var item = new TimeScaleLabelItem();

                    item.cssClass = time % 2 == 1 ? "odd-label" : "even-label";
                    item.labelText = now.AddHours(time).ToString("hhtt").ToLower();

                    if (time == timeRange.StartTime || time == timeRange.EndTime - 1)
                    {
                        item.divStyle = $"max-width:{cellWidthPercent - 1}%;width:{cellWidthPercent - 1}%";
                    }
                    else if (time == timeRange.EndTime)
                    {
                        item.divStyle = $"max-width:1%;width:1%";
                    }
                    else
                    {
                        item.divStyle = $"max-width:{cellWidthPercent}%;width:{cellWidthPercent}%";
                    }
                    labels.Add(item);
                }
                TimeScaleLabel = labels.ToArray();

                var items = new List<TimeScaleItem>();
                for (int i = 0, count = (int)timeRange.Span; i < count; i++)
                {
                    var item = new TimeScaleItem
                    {
                        lineonex = $"{i * cellWidthPercent}%",
                        lineoney = "16",
                        linetwox = $"{i * cellWidthPercent + cellWidthPercent / 2}%",
                        linetwoy = "8"
                    };
                    items.Add(item);
                }

                TimeScale = items.ToArray();

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
                        item.labeltooltip = tl.ElapsedTime.ToString();

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
