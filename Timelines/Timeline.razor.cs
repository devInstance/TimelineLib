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
        private GuidingLine[] GuidingLines;

        private IScopeLog log;

        protected override void OnInitialized()
        {
            log = ScopeManager.CreateLogger(this);
            using (var l = log.TraceScope())
            {
                if (!InitializeTimeScaleGuard)
                {
                    InitializeTimeScaleGuard = true;

                    CalculateTimeRange();

                    InitializeTimeScale();
                    InitializeTimeBars();
                    InitializeTimeScaleGuard = false;
                }
            }
        }

        protected override void OnParametersSet()
        {
            using (var l = log.TraceScope())
            {
                base.OnParametersSet();

                if (!InitializeTimeScaleGuard)
                {
                    InitializeTimeScaleGuard = true;
                    CalculateTimeRange();

                    InitializeTimeScale();
                    InitializeTimeBars();
                    StateHasChanged();
                    InitializeTimeScaleGuard = false;
                }
            }
        }

        private void CalculateTimeRange()
        {
            using (var l = log.TraceScope())
            {
                if (timeRange != null)
                {
                    l.T($"Existing {timeRange.StartTime} - {timeRange.EndTime}");
                }
                l.T($"Parent {Parent.Range.StartTime} - {Parent.Range.EndTime}");
                if (timeRange != Parent.Range)
                {
                    timeRange = Parent.Range;

                    l.T($"New range from parent {timeRange.StartTime} - {timeRange.EndTime}");

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
            using (var l = log.TraceScope())
            {
                //if (!InitializeTimeScaleGuard)
                //{
                //    InitializeTimeScaleGuard = true;
                int height = 0;
                var now = DateTime.Today;
                CalculateTimeRange();

                if (Data != null)
                {
                    height = Data.Count() * 36 + 10; //TODO: introduce parameter and default value
                }
                else
                {
                    height = 46;
                }

                Parent.UpdateParentsHeight(height);
                //    InitializeTimeScaleGuard = false;
                //}
            }
        }

        private void InitializeTimeBars()
        {
            using (var l = log.TraceScope())
            {
                var bars = new List<TimeBar>();
                var glines = new List<GuidingLine>();
                var lineLabels = new List<TimeScaleLabelItem>();

                int height = 26; //TODO: introduce parameter and default value

                if (Data != null)
                {
                    int n = 0;
                    foreach (var task in Data)
                    {
                        int y = n * 36 + 10;  //TODO: introduce parameter and default value
                        var gline = new GuidingLine
                        {
                            Horizontal = $"{y + (height / 2)}",
                            Left = "0%",
                            Right = "100%" //TODO: Introduce margins
                        };
                        glines.Add(gline);

                        var lineLabel = new TimeScaleLabelItem();
                        lineLabel.LabelText = task.Title; //TODO: needs i18n review
                        lineLabel.DivStyle = n == 0 ? $"height:46px; margin-top:10px;padding-top:10px;" : $"height:36px";
                        lineLabels.Add(lineLabel);

                        foreach (var tl in task.Items)
                        {
                            float x = ((tl.StartTime.Hour + (tl.StartTime.Minute / 60.0f) - (float)timeRange.StartTime) * cellWidthPercent);
                            float width = ((float)tl.ElapsedTime.TotalMinutes / 60.0f * cellWidthPercent);
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
                            item.Tooltip = String.Format(timeFormat, String.IsNullOrEmpty(tl.Description) ? task.Title:tl.Description, tm);

                            bars.Add(item);
                        }

                        n++;
                    }
                }

                TimeBars = bars.ToArray();
                GuidingLines = glines.ToArray();

                //if (!InitializeTimeScaleGuard)
                //{
                //    InitializeTimeScaleGuard = true;

                Parent.SetLineLabels(lineLabels.ToArray());
                //    InitializeTimeScaleGuard = false;
                //}
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
            using (var l = log.TraceScope())
            {
                //InitializeTimeScale();
                var res = base.ShouldRender();
                l.T($"***result = ${res}");
                return res;
            }
        }

    }
}
