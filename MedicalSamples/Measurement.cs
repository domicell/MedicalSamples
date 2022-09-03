using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalSamples
{
    public enum MeasurementType
    {
        TEMP,   //Temperature
        RATE,   //HeartRate,
        SPO2    //SpO2
    }
    public class Measurement
    {
        /// <summary>
        /// Original sample time
        /// </summary>
        public DateTime measurementTime;
        public Double measurementValue;
        public MeasurementType type;

        /// <summary>
        /// Rounded sample time to nearest 5 minutes: 00:00, 05:00, 10:00, ...
        /// </summary>
        public DateTime SampleTime
        {
            get
            {
                DateTime d = measurementTime;
                if (!((d.Minute % 5) == 0 && d.Second == 0)) //do not add 5 minutes if 00:00, 05:00, 10:00 
                {
                    d = new DateTime(d.Year, d.Month, d.Day, d.Hour, (d.Minute / 5) * 5, 0);
                    d = d.AddMinutes(5);
                }
                return d;
            }
        }
    }
}
