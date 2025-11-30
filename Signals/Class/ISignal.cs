using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signals
{
    public interface ISignal
    {
        double Amplitude { get; set; }
        double Frequency { get; set; }
        int PointsCount { get; set; }
        double SampleRate { get; set; }

        double[] Generate();
    }
}
