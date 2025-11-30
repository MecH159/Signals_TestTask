using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signals
{
    public class Meandr : SignalBase
    {
        public override double[] Generate()
        {
            double[] signal = new double[PointsCount];

            for (int i = 0; i < PointsCount; i++)
            {
                double time = i / SampleRate;
                double phase = 2 * Math.PI * Frequency * time;
                signal[i] = Math.Sin(phase) >= 0 ? Amplitude : -Amplitude;
            }

            return signal;
        }
    }
}
