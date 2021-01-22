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
    public partial class Heatline
    {
        private ILogProvider LogProvider = new ConsoleLogProvider(LogLevel.DEBUG_EXTRA); //TODO: refactor

        private DateTime currentDateTime = DateTime.Now;

        private TimeRange timeRange;
        private float cellWidthPercent;

        private TimeScaleLabelItem[] TimeScaleLabel;
        private TimeScaleItem[] TimeScale;
        private HeatItem[] HeatItems;

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
                InitializeHeatlines();
                StateHasChanged();
            }
        }

        private void CalculateTimeRange()
        {
            timeRange = TimeRangeCalculator.CreateTimeRange(StartTime, EndTime);

            if (IsTimeRangeFlexible)
            {
                //timeRange = TimeRangeCalculator.CalculateDynamicTimeRange(timeRange, Data);
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

        private void InitializeHeatlines()
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
                            cssClass = (line.CssClass != null ? line.CssClass.ToLower() : "white"),
                            x = $"{x}%",
                            y = $"{y}",
                            value = it.Value
                        };

                        heatlines.Add(item);
                    }

                    n++;
                }
            }

            HeatItems = heatlines.ToArray();
        }


        protected override bool ShouldRender()
        {
            using (var l = log.DebugExScope())
            {
                InitializeTimeScale();
                InitializeHeatlines();

                return true;
            }
        }

    }
}
