using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevInstance.TimelineLib;

namespace DevInstance.Timeline.Sample.Pages
{
    public partial class Index
    {
        private TimelineLib.Timeline.Line[] Lines;

        protected async override Task OnInitializedAsync()
        {
            var now = new DateTime(2021, 1, 12, 0, 0, 0);

            Lines = new TimelineLib.Timeline.Line[] {
                new TimelineLib.Timeline.Line 
                { 
                    Title = "test" ,
                    CssClass = "orange",
                    Items = new TimelineLib.Timeline.Item[]
                    {
                        new TimelineLib.Timeline.Item { StartTime = now, ElapsedTime = new TimeSpan(0, 30, 0)},
                        new TimelineLib.Timeline.Item { StartTime = now.AddHours(2), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new TimelineLib.Timeline.Item { StartTime = now.AddHours(4), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new TimelineLib.Timeline.Item { StartTime = now.AddHours(8), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new TimelineLib.Timeline.Item { StartTime = now.AddHours(12), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new TimelineLib.Timeline.Item { StartTime = now.AddHours(20), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new TimelineLib.Timeline.Item { StartTime = now.AddHours(23), ElapsedTime = new TimeSpan(0, 30, 0)},
                    }
                },
                new TimelineLib.Timeline.Line
                {
                    Title = "test 2" ,
                    CssClass = "blue",
                    Items = new TimelineLib.Timeline.Item[]
                    {
                        new TimelineLib.Timeline.Item { StartTime = now.AddHours(3), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new TimelineLib.Timeline.Item { StartTime = now.AddHours(11), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new TimelineLib.Timeline.Item { StartTime = now.AddHours(16.25), ElapsedTime = new TimeSpan(0, 30, 0)}
                    }
                }

            };
        }
    }
}
