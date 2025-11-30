using Newtonsoft.Json.Linq;
using Signals;
using System;
using Xunit;

namespace TestsSignal
{
    public class StatisticsTest 
    {
        [Fact]
        public void StatisticSignal_ReturnsCorrectValues()
        {
            // Arrange
            var sinusoid = new Sinusoid();
            double amplitude = 5;
            double frequency = 20;
            int pointsCount = 103;

            // Act
            sinusoid.SignalGeneratorBase(amplitude, frequency, pointsCount);

            double [] points = sinusoid.Generate();
            double max = points.Max();
            double min = points.Min();
            double sum = 0;
            foreach (var point in points)
            {
                sum += point;
            }

            int zeroCrossing = 0;
            for (int i = 1; i < points.Length; i++)
            {
                if (points[i - 1] * points[i] < 0)
                {
                    zeroCrossing++;
                }
            }
            // Assert
            Assert.Equal(4.99, Math.Round(max,2));
            Assert.Equal(-4.99, Math.Round(min,2));
            Assert.Equal(1.87, Math.Round(sum, 2));
            Assert.Equal(4, zeroCrossing);
            Assert.All(points, value => Assert.InRange(value, -amplitude, amplitude));
        }

    }
}