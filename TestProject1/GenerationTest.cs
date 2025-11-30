using Xunit;
using Signals;
using System;

namespace TestsSignal
{
    public class GenerationTest
    {
        [Fact]
        public void Sinusoid_Generate_CorrectValues()
        {
            // Arrange
            var sinusoid = new Sinusoid();
            double amplitude = 1.0;
            double frequency = 1.0;
            int pointsCount = 1000;

            // Act
            sinusoid.SignalGeneratorBase(amplitude, frequency, pointsCount);
            var result = sinusoid.Generate();

            // Assert
            Assert.Equal(pointsCount, result.Length);
            Assert.All(result, value => Assert.InRange(value, -amplitude, amplitude));

            // Первое значение должно быть близко к 0 (sin(0) = 0)
            Assert.True(Math.Abs(result[0]) < 0.01);
        }

        [Fact]
        public void Meandr_Generate_CorrectValues()
        {
            // Arrange
            var meandr = new Meandr();
            double amplitude = 5;
            double frequency = 10;
            int pointsCount = 1000;

            // Act
            meandr.SignalGeneratorBase(amplitude, frequency, pointsCount);
            var result = meandr.Generate();

            // Assert
            Assert.Equal(pointsCount, result.Length);

            // Меандр должен принимать только значения +amplitude или -amplitude
            Assert.All(result, value =>
                Assert.True(value == amplitude || value == -amplitude));

            // Меандр должен менять значение при переходе
            bool hasTransitions = false;
            for (int i = 1; i < Math.Min(100, result.Length); i++)
            {
                if (result[i] != result[i - 1])
                {
                    hasTransitions = true;
                    break;
                }
            }
            Assert.True(hasTransitions);
        }


    }
}