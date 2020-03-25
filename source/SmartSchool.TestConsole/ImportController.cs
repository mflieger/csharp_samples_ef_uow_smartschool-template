using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SmartSchool.Core.Entities;
using Utils;

namespace SmartSchool.TestConsole
{
    public class ImportController
    {
        const string Filename = "measurements.csv";

        /// <summary>
        /// Liefert die Messwerte mit den dazugehörigen Sensoren
        /// </summary>
        public static IEnumerable<Measurement> ReadFromCsv()
        {
            string filePath = MyFile.GetFullNameInApplicationTree(Filename);
            string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

            IList<Measurement> measurements = new List<Measurement>();
            IDictionary<string, Sensor> sensors = new Dictionary<string, Sensor>();
            bool firstrow = true;

            foreach(var line in lines)
            {
                if (!firstrow)
                {
                    string[] parts = line.Split(";");
                    string[] locationName = parts[2].Split("_");
                    string name = locationName[0];
                    string location = locationName[1];
                    DateTime dateTime = DateTime.Parse($"{parts[0]} {parts[1]}");
                    Measurement measurement = new Measurement() { Time = dateTime, Value = Convert.ToDouble(parts[3]) };

                    if (!sensors.ContainsKey(parts[2]))
                    {
                        Sensor newSensor = new Sensor() { Name = name, Location = location };
                        measurement.Sensor = newSensor;
                        sensors.Add(parts[2], newSensor);
                        newSensor.Measurements.Add(measurement);
                    }
                    else
                    {
                        Sensor existingSensor = sensors
                                                .Values
                                                .SingleOrDefault(s => s.Name == name && s.Location == location);
                        measurement.Sensor = existingSensor;
                        existingSensor.Measurements.Add(measurement);
                    }
                    measurements.Add(measurement);
                }
                else
                {
                    firstrow = false;
                }
            }

            return measurements;
        }

    }
}
