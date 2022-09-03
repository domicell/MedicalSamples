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
        public Dictionary<MeasurementType, List<Measurement>> sample(
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
