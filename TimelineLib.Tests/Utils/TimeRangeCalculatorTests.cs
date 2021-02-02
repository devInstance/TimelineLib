using DevInstance.TimelineLib.Utils;
using Xunit;
using System;

namespace DevInstance.TimelineLib.Utils.Tests
{
    public class TimeRangeCalculatorTests
    {
        [Theory]
        [InlineData(null, null, 8.0, 20.0)]
        [InlineData(9.5, 12.25, 9.5, 12.25)]
        [InlineData(null, 12.25, 8.0, 12.25)]
        [InlineData(9.5, null, 9.5, 20.0)]
        public void TimelineCreateTimeRangeValuesTest(double? start, double? end, double expectedStart, double expectedEnd)
        {
            var result = TimeRangeCalculator.CreateTimeRange(start, end);

            Assert.NotNull(result);
            Assert.Equal(expectedStart, result.StartTime);
            Assert.Equal(expectedEnd, result.EndTime);
        }

        [Theory]
        [InlineData(8.0, 20.0, new double[] { 9, 13, 15 }, new int[] { 30, 30, 30 }, 8.0, 20.0)]
        [InlineData(8.0, 20.0, new double[] { 9, 13, 20.5 }, new int[] { 30, 30, 30 }, 8.0, 22.0)]
        [InlineData(8.0, 20.0, new double[] { 2, 4, 20.75 }, new int[] { 30, 30, 30 }, 2.0, 22.0)]
        [InlineData(8.0, 20.0, new double[] { 9, 13, 22 }, new int[] { 30, 30, 30 }, 8.0, 23)]
        [InlineData(8.0, 20.0, new double[] { 2, 4, 12 }, new int[] { 30, 30, 90 }, 2.0, 20.0)]
        public void TimelineCalculateDynamicTimeRangeInTheRangeTest(double start, double end, double[] timeEntries, int[] timeSpanEntries, double expectedStart, double expectedEnd)
        {
            var now = new DateTime(2021, 1, 12, 0, 0, 0);

            var data = new Timeline.Line[] {
                new Timeline.Line
                {
                    Title = "test" ,
                    CssClass = "orange",
                    Items = new Timeline.Item[]
                    {
                        new Timeline.Item { StartTime = now.AddDays(-30).AddHours(timeEntries[0]), ElapsedTime = new TimeSpan(0, timeSpanEntries[0], 0)},
                        new Timeline.Item { StartTime = now.AddDays(-15).AddHours(timeEntries[1]), ElapsedTime = new TimeSpan(0, timeSpanEntries[1], 0)},
                        new Timeline.Item { StartTime = now.AddHours(timeEntries[2]), ElapsedTime = new TimeSpan(0, timeSpanEntries[2], 0)}
                    }
                },
                new Timeline.Line
                {
                    Title = "test 2" ,
                    CssClass = "blue",
                    Items = new Timeline.Item[]
                    {
                        new Timeline.Item { StartTime = now.AddHours(9), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new Timeline.Item { StartTime = now.AddDays(-20).AddHours(11), ElapsedTime = new TimeSpan(0, 30, 0)},
                        new Timeline.Item { StartTime = now.AddHours(12), ElapsedTime = new TimeSpan(0, 30, 0)}
                    }
                }

            };

            var result = TimeRangeCalculator.CalculateDynamicTimeRange(new TimeRange(start, end), data);

            Assert.NotNull(result);
            Assert.Equal(expectedStart, result.StartTime);
            Assert.Equal(expectedEnd, result.EndTime);
        }

        [Theory]
        [InlineData(8.0, 20.0, new double[] { 9, 13, 15 }, 8.0, 20.0)]
        [InlineData(8.0, 20.0, new double[] { 9, 13, 20.5 }, 8.0, 22.0)]
        [InlineData(8.0, 20.0, new double[] { 2, 4, 20.75 }, 2.0, 22.0)]
        [InlineData(8.0, 20.0, new double[] { 9, 13, 22 }, 8.0, 23)]
        [InlineData(8.0, 20.0, new double[] { 2, 4, 12 }, 2.0, 20.0)]
        public void HeatlineCalculateDynamicTimeRangeTest(double start, double end, double[] timeEntries, double expectedStart, double expectedEnd)
        {
            var now = new DateTime(2021, 1, 12, 0, 0, 0);

            var data = new Heatline.Line[] {
                new Heatline.Line
                {
                    Title = "test" ,
                    CssClass = "orange",
                    Items = new Heatline.Item[]
                    {
                        new Heatline.Item { Time = now.AddHours(timeEntries[0]) },
                        new Heatline.Item { Time = now.AddHours(timeEntries[1]) },
                        new Heatline.Item { Time = now.AddHours(timeEntries[2]) }
                    }
                }
            };

            var result = TimeRangeCalculator.CalculateDynamicTimeRange(new TimeRange(start, end), 0.15, data);

            Assert.NotNull(result);
            Assert.Equal(expectedStart, result.StartTime);
            Assert.Equal(expectedEnd, result.EndTime);
        }
    }
}