using System;
using System.Threading.Tasks;
using static DevInstance.Timelines.Timeline;

namespace DevInstance.Timeline.Sample.Pages
{
    public partial class TimelineTest
    {
        private Line[] Lines;

        protected async override Task OnInitializedAsync()
        {
            var now = new DateTime(2021, 1, 12, 0, 0, 0);

            Lines = new Line[] {
                new Line 
                { 
                    Title = "test" ,
                    CssClass = "orange",
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
                },
                new Line
                {
                    Title = "test 2" ,
                    CssClass = "blue",
                    Items = new Item[]
                    {
                        new Item { StartTime = now.AddHours(3), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new Item { StartTime = now.AddHours(11), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new Item { StartTime = now.AddHours(16.25), ElapsedTime = new TimeSpan(0, 30, 0)}
                    }
                }

            };
        }
    }
}
