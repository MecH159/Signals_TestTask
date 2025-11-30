using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signals
{
    public class Sinusoid : SignalBase
    {
        
        public override double[] Generate()
        {
            double[] signal = new double[PointsCount];

            for (int i = 0; i < PointsCount; i++)
            {
                double time = i / SampleRate;
                signal[i] = Amplitude * Math.Sin(2 * Math.PI * Frequency * time);
            }

            return signal;
        }

    }
}
