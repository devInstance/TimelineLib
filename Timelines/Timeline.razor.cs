using DevInstance.LogScope;
using DevInstance.Timelines.Model;
using DevInstance.Timelines.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevInstance.Timelines
{
    public partial class Timeline
    {
        private DateTime currentDateTime = DateTime.Now;

        private TimeRange timeRange;
        private float cellWidthPercent;

        private TimeBar[] TimeBars;

        private IScopeLog log;

        protected override void OnInitialized()
        {
            log = ScopeManager.CreateLogger(this);
            using (var l = log.DebugExScope())
            {
                CalculateTimeRange();

                InitializeTimeScale();
                InitializeTimeBars();
            }
        }

        protected override void OnParametersSet()
        {
            using (var l = log.DebugExScope())
            {
                base.OnParametersSet();

                CalculateTimeRange();

                InitializeTimeScale();
                InitializeTimeBars();
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

                    l.DE($"New range from parent {timeRange.StartTime} - {timeRange.EndTime}");

                    if (Parent.IsTimeRangeFlexible)
                    {
                        timeRange = TimeRangeCalculator.CalculateDynamicTimeRange(timeRange, Data);
                    }

                    cellWidthPercent = 100.0f / (float)timeRange.Span;

                    Parent.UpdateParentsTimeRange(timeRange);
                }
            }
        }

        bool InitializeTimeScaleGuard = false;
        private void InitializeTimeScale()
        {
            using (var l = log.DebugExScope())
            {
                if (!InitializeTimeScaleGuard)
                {
                    InitializeTimeScaleGuard = true;
                    int height = 0;
                    var now = DateTime.Today;
                    CalculateTimeRange();

                    if (Data != null)
                    {
                        height = Data.Count() * 36 + 10; //TODO: refactor
                    }
                    else
                    {
                        height = 46;
                    }

                    Parent.UpdateParentsHeight(height);
                    InitializeTimeScaleGuard = false;
                }
            }
        }

        private void InitializeTimeBars()
        {
            using (var l = log.DebugExScope())
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
                            item.Tooltip = String.Format(timeFormat, tl.Description, tm);

                            bars.Add(item);
                        }

                        n++;
                    }
                }

                TimeBars = bars.ToArray();
            }
        }


        protected override bool ShouldRender()
        {
            //using (var l = log.DebugExScope())
            //{
            //    InitializeTimeScale();
            //    InitializeTimeBars();

            //    return true;
            //}
            using (var l = log.DebugExScope())
            {
                //InitializeTimeScale();
                var res = base.ShouldRender();
                l.DE($"***result = ${res}");
                return res;
            }
        }

    }
}
