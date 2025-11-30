using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Signals
{
    public abstract class SignalBase : ISignal
    {
        public double Amplitude { get; set; }
        public double Frequency { get; set; }
        public int PointsCount { get; set; }
        public double SampleRate { get; set; }

        public void SignalGeneratorBase(double amplitude, double frequency,
                                int pointsCount, double sampleRate = 1000)
        {
            Amplitude = amplitude;
            Frequency = frequency;
            PointsCount = pointsCount;
            SampleRate = sampleRate;

            ValidateParameters();
        }

        protected virtual void ValidateParameters()
        {
            if (Amplitude <= 0)
                throw new ArgumentException("Амплитуда должна быть положительной");
            if (Frequency <= 0)
                throw new ArgumentException("Частота должна быть положительной");
            if (PointsCount < 100 || PointsCount >= 10000)
                throw new ArgumentException("Количество точек должно быть минимум 100 и максимум 10000");

            // Проверка теоремы Найквиста
            if (SampleRate <= 2 * Frequency)
                throw new ArgumentException(
                    $"Частота дискретизации (1000) ({SampleRate} Гц) должна быть больше " +
                    $"удвоенной частоты сигнала ({2 * Frequency} Гц)");
        }

        public abstract double[] Generate();
    }
}
