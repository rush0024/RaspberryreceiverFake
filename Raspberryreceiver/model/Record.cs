using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspberryreceiver.model
{
   
        public class Record
        {
            public double Pressure { get; set; }

            public double Temperature { get; set; }

            public double Humidity { get; set; }


            public Record(double pressure, double temperature, double humidity)
            {
                Humidity = humidity;
                Pressure = pressure;
                Temperature = temperature;

            }
        }
    
}

