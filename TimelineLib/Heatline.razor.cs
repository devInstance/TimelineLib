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
        private TimeRange timeRange;
        private float cellWidthPercent;

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
