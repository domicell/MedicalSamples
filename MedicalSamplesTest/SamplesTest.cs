using NUnit.Framework;
using MedicalSamples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MedicalSamplesTest
{
    public class Tests
    {
        
        List<Measurement>? listMeasurements;

        [SetUp]
        public void Setup()
        {
            //load sample data
            listMeasurements = new List<Measurement>()
            {
                new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:00:00"), type = MeasurementType.TEMP, measurementValue = 235.79 },
                new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:59:59"), type = MeasurementType.TEMP, measurementValue = 335.79 },
                new Measurement(){ measurementTime = DateTime.Parse("2021-02-28T23:59:59"), type = MeasurementType.TEMP, measurementValue = 435.79 },

                new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:04:45"), type = MeasurementType.TEMP, measurementValue = 35.79 },
                new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:01:18"), type = MeasurementType.SPO2, measurementValue = 98.78 },
                new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:09:07"), type = MeasurementType.TEMP, measurementValue = 35.01 },
                new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:03:34"), type = MeasurementType.SPO2, measurementValue = 96.49 },
                new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:02:01"), type = MeasurementType.TEMP, measurementValue = 35.82 },
                new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:05:00"), type = MeasurementType.SPO2, measurementValue = 97.17 },
                new Measurement(){ measurementTime = DateTime.Parse("2017-01-03T10:05:01"), type = MeasurementType.SPO2, measurementValue = 95.08 }
            };
        }

        [Test]
        public void OnlyOneRecordPerTypePer5min()
        {
            var flat = GetFlatList();

            var grouped = flat.GroupBy(s => new { s.type, s.SampleTime }).ToList();

            //flattened list have the same count as groupped list
            Assert.IsTrue(flat.Count == grouped.Count);
        }

        [Test]
        public void TimeStampMultiply5minSecondsIsZero()
        {
            var flat = GetFlatList();

            //list should be empty if all SampleTime minutes are multiply of 5 and seconds are 0
            var dtHaveReminder = flat.Where(s=>s.SampleTime.Minute % 5 != 0 || s.SampleTime.Second != 0).ToList();

            Assert.IsTrue(dtHaveReminder.Count == 0);
        }
        
        [Test]
        public void TimeStampMoreThanSampleOriginalTime()
        {
            var flat = GetFlatList();

            //list should be empty if all SampleTime are greater or equal measurementTime
            var dt = flat.Where(s => s.SampleTime < s.measurementTime).ToList();

            Assert.IsTrue(dt.Count == 0);
        }

        [Test]
        public void MeasurementTimeMultiply5minSecondsIsZeroSameInterval()
        {
            var flat = GetFlatList();

            //list should be empty if all SampleTime minutes are multiply of 5 and seconds are 0
            var dtHaveNoReminder = flat.Where(s => s.measurementTime.Minute % 5 == 0 && s.measurementTime.Second == 0).ToList();

            var notEqualMeasureAndSample = dtHaveNoReminder.Where(s => s.measurementTime != s.SampleTime).ToList();

            Assert.IsTrue(notEqualMeasureAndSample.Count == 0);
        }

        [Test]
        public void IsSorted()
        {
            var listSamples = GetSamples();
            foreach (var samples in listSamples)
                Assert.That(samples.Value, Is.Ordered.By("SampleTime"));
        }

        /// <summary>
        /// Generate sample data
        /// </summary>
        Dictionary<MeasurementType, List<Measurement>> GetSamples()
        {
            if (listMeasurements == null || listMeasurements.Count == 0)
            {
                Console.WriteLine($"{MethodBase.GetCurrentMethod()?.Name}: No records in sequence");
                return new Dictionary<MeasurementType, List<Measurement>>();
            }

            Measurements measurements = new Measurements();
            DateTime startTime = listMeasurements.Min(x => x.measurementTime);
            var result = measurements.sample(startTime, listMeasurements);
            return result;
        }

        /// <summary>
        /// Prepare data for test cases
        /// </summary>
        /// <returns>flatten dictionary</returns>
        List<Measurement> GetFlatList()
        {
            var result = GetSamples();
            var flat = result.SelectMany(s => s.Value).ToList(); //flatten dictionary
            return flat;
        }
    }
}