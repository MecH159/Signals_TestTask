using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signals
{
    public class SawTooth : SignalBase
    {
        public override double[] Generate()
        {
            double[] signal = new double[PointsCount];
            double period = 1.0 / Frequency;
            for (int i = 0; i < PointsCount; i++)
            {

                double time = i / SampleRate;
                double positionInPeriod = (time % period) / period;
                signal[i] = 2 * Amplitude * positionInPeriod - Amplitude;
            }

            return signal;
        }
    }
}
