using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSamples
{
    enum MeasurementType
    {
        Temperature,
        HeartRate,
        SpO2
    }
    class Measurement
    {
        private DateTime measurementTime;
        private Double measurementValue;
        private MeasurementType type;
    }
}
