using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DevInstance.Timelines.Timeline;

namespace DevInstance.Timeline.Sample.Pages
{
    public partial class TimelineTest
    {
        private Line[] Lines;
        private List<Line> CustomLines;
        private List<Line> DynamicLines;
        private List<Item> DynamicItems;
        private Line NewLine;
        private Line DynamicLine;

        protected async override Task OnInitializedAsync()
        {
            var now = new DateTime(2021, 1, 12, 0, 0, 0);

            var line1 = new Line
            {
                Title = "test 2",
                CssClass = "blue",
                Items = new Item[]
                    {
                        new Item { StartTime = now, ElapsedTime = new TimeSpan(0, 30, 0)},
                        new Item { StartTime = now.AddHours(2), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new Item { StartTime = now.AddHours(4), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new Item { StartTime = now.AddHours(8), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new Item { StartTime = now.AddHours(12), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new Item { StartTime = now.AddHours(20), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new Item { StartTime = now.AddHours(23), ElapsedTime = new TimeSpan(0, 30, 0)},
                    }
            };

            var line2 = new Line
            {
                Title = "test",
                CssClass = "orange",
                Items = new Item[]
                {
                    new Item { StartTime = now.AddHours(3), ElapsedTime = new TimeSpan(0, 30, 0)},
                    new Item { StartTime = now.AddHours(11), ElapsedTime = new TimeSpan(0, 30, 0)},
                    new Item { StartTime = now.AddHours(16.25), ElapsedTime = new TimeSpan(0, 30, 0)}
                }
            };

            Lines = new Line[] { line1, line2 };

            CustomLines = new List<Line>();

            NewLine = new Line
            {
                Title = "test 3",
                CssClass = "green",
                Items = new Item[]
                {
                        new Item { StartTime = now.AddHours(5), ElapsedTime = new TimeSpan(1, 15, 0)},
                        new Item { StartTime = now.AddHours(10.3), ElapsedTime = new TimeSpan(0, 49, 0)},
                        new Item { StartTime = now.AddHours(15.25), ElapsedTime = new TimeSpan(0, 20, 0)}
                }
            };

            CustomLines.Add(line2);
            CustomLines.Add(line1);

            DynamicItems = new List<Item>();
            DynamicLine = new Line
            {
                Title = "test 4",
                CssClass = "red",
                Items = DynamicItems
            };

            DynamicLines = new List<Line>();
            DynamicLines.Add(DynamicLine);
        }

        private async void OnAddItem()
        {
            var now = new DateTime(2021, 1, 12, 0, 0, 0);

            double start;
            Double.TryParse(startTimeLabel, out start);

            int time;
            Int32.TryParse(endTimeLabel, out time);

            DynamicItems.Add(new Item { StartTime = now.AddHours(start), ElapsedTime = new TimeSpan(0, time, 0) });

            StateHasChanged();
        }

    }
}
