using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevInstance.Timelines;

namespace DevInstance.Timeline.Sample.Pages
{
    public partial class HeatlineTest
    {
        private Timelines.Heatline.Line[] Lines;

        protected async override Task OnInitializedAsync()
        {
            var now = new DateTime(2021, 1, 12, 0, 0, 0);

            Lines = new Timelines.Heatline.Line[] {
                new Timelines.Heatline.Line
                {
                    Title = "test" ,
                    CssClass = "orange",
                    Items = new Timelines.Heatline.Item[]
                    {
                        new Timelines.Heatline.Item { Time = now, Value = 0.1f },
                        new Timelines.Heatline.Item { Time = now.AddHours(2), Value = 0.1f },
                        new Timelines.Heatline.Item { Time = now.AddHours(4), Value = 0.2f },
                        new Timelines.Heatline.Item { Time = now.AddHours(8), Value = 0.4f },
                        new Timelines.Heatline.Item { Time = now.AddHours(12), Value = 0.8f },
                        new Timelines.Heatline.Item { Time = now.AddHours(20), Value = 1.0f },
                        new Timelines.Heatline.Item { Time = now.AddHours(23), Value = 0.1f },
                    }
                },
                new Timelines.Heatline.Line
                {
                    Title = "test" ,
                    CssClass = "blue",
                    Items = new Timelines.Heatline.Item[]
                    {
                        new Timelines.Heatline.Item { Time = now, Value = 0.1f },
                        new Timelines.Heatline.Item { Time = now.AddHours(2.25), Value = 0.1f },
                        new Timelines.Heatline.Item { Time = now.AddHours(4.25), Value = 0.2f },
                        new Timelines.Heatline.Item { Time = now.AddHours(8.0), Value = 0.6f },
                        new Timelines.Heatline.Item { Time = now.AddHours(8.1), Value = 0.4f },
                        new Timelines.Heatline.Item { Time = now.AddHours(8.2), Value = 0.3f },
                        new Timelines.Heatline.Item { Time = now.AddHours(8.3), Value = 0.8f },
                        new Timelines.Heatline.Item { Time = now.AddHours(8.4), Value = 0.9f },
                        new Timelines.Heatline.Item { Time = now.AddHours(8.5), Value = 0.2f },
                        new Timelines.Heatline.Item { Time = now.AddHours(8.6), Value = 0.4f },
                        new Timelines.Heatline.Item { Time = now.AddHours(12.00), Value = 0.8f },
                        new Timelines.Heatline.Item { Time = now.AddHours(12.25), Value = 0.9f },
                        new Timelines.Heatline.Item { Time = now.AddHours(12.50), Value = 0.6f },
                        new Timelines.Heatline.Item { Time = now.AddHours(12.75), Value = 0.4f },
                        new Timelines.Heatline.Item { Time = now.AddHours(20.50), Value = 1.0f },
                        new Timelines.Heatline.Item { Time = now.AddHours(23), Value = 0.1f },
                    }
                },
            };
        }
    }
}
