using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace MedicalSamples
{
    public class Measurements
    {
        public static List<Measurement> GetTestMeasurements 
        {
            get
            {
                //load sample data
                List<Measurement> listMeasurements = new ()
                {
                    //new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:00:00"), type = MeasurementType.TEMP, measurementValue = 235.79 },
                    //new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:59:59"), type = MeasurementType.TEMP, measurementValue = 335.79 },
                    //new Measurement(){ measurementTime = DateTime.Parse("2021-02-28T23:59:59"), type = MeasurementType.TEMP, measurementValue = 435.79 },

                    new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:04:45"), type = MeasurementType.TEMP, measurementValue = 35.79 },
                    new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:01:18"), type = MeasurementType.SPO2, measurementValue = 98.78 },
                    new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:09:07"), type = MeasurementType.TEMP, measurementValue = 35.01 },
                    new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:03:34"), type = MeasurementType.SPO2, measurementValue = 96.49 },
                    new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:02:01"), type = MeasurementType.TEMP, measurementValue = 35.82 },
                    new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:05:00"), type = MeasurementType.SPO2, measurementValue = 97.17 },
                    new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:05:01"), type = MeasurementType.SPO2, measurementValue = 95.08 }
                };
                return listMeasurements;
            }            
        }
        public static Dictionary<MeasurementType, List<Measurement>> sample(
            DateTime startOfSampling, List<Measurement> unsampledMeasurements)
        {
            if (unsampledMeasurements == null || unsampledMeasurements.Count == 0)
            {
                Console.WriteLine($"{MethodBase.GetCurrentMethod()?.Name}: No records in sequence");
                return new Dictionary<MeasurementType, List<Measurement>>();
            }

            //grouping by SampleTime (00:00, 05:00, ...)
            //and select last samples for each group
            var lst = unsampledMeasurements.Where(sample => sample.measurementTime >= startOfSampling)
                                           .GroupBy(sample => new { sample.type, sample.SampleTime })
                                           .Select(grp => grp.Aggregate((max, cur) =>
                                           (max == null || cur.measurementTime > max.measurementTime) ? cur : max))
                                           .OrderBy(o => o.SampleTime);

            //group according required output
            var result = lst.GroupBy(sample => sample.type).ToDictionary(sample => sample.Key, samples => samples.ToList());
            
            return result;
        }
    }
}
