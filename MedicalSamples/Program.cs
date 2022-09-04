using MedicalSamples;
using System.Collections.Generic;
using System.Linq;

Console.WriteLine("Measurement Sorting\n\n");

List<Measurement> listMeasurements = Measurements.GetTestMeasurements;

//print all results to console
var samples = Measurements.sample(DateTime.MinValue, listMeasurements);
foreach (var type in samples)
    foreach (Measurement m in type.Value)
        Console.WriteLine($"{{{m.SampleTime.ToString("yyyy-MM-ddTHH:mm:ss")},{m.type},{m.measurementValue}}}");
