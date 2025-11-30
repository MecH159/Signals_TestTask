using Xunit;
using Signals;
using System;

namespace TestsSignal
{
    public class ValidationTest
    {
        [Theory]
        [InlineData(0)]      // нулевая амплитуда
        [InlineData(-1.0)]   // отрицательная амплитуда
        public void SignalBase_InvalidAmplitude_ThrowsException(double amplitude)
        {
            // Arrange
            var signal = new Sinusoid();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                signal.SignalGeneratorBase(amplitude, 1.0, 1000));

            Assert.Contains("Амплитуда должна быть положительной", exception.Message);
        }

        [Theory]
        [InlineData(0)]      // нулевая частота
        [InlineData(-1.0)]   // отрицательная частота
        public void SignalBase_InvalidFrequency_ThrowsException(double frequency)
        {
            // Arrange
            var signal = new Sinusoid();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                signal.SignalGeneratorBase(1.0, frequency, 1000));

            Assert.Contains("Частота должна быть положительной", exception.Message);
        }

        [Theory]
        [InlineData(50)]     // слишком мало точек
        [InlineData(99)]     // слишком мало точек
        [InlineData(10000)]  // слишком много точек
        [InlineData(15000)]  // слишком много точек
        public void SignalBase_InvalidPointsCount_ThrowsException(int pointsCount)
        {
            // Arrange
            var signal = new Sinusoid();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                signal.SignalGeneratorBase(1.0, 1.0, pointsCount));

            Assert.Contains("Количество точек должно быть минимум 100 и максимум 10000",
                exception.Message);
        }

        [Fact]
        public void SignalBase_VeryHighFrequency_ThrowsException()
        {
            // Arrange
            var signal = new Sinusoid();
            double highFrequency = 600; // Выше половины частоты дискретизации (1000)

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                signal.SignalGeneratorBase(1.0, highFrequency, 1000));

            Assert.Contains("Частота дискретизации", exception.Message);
        }
    }
}