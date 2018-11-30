using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terra.Models
{
    public class Status
    {
        public int Id { get; set; }
        public float Temp { get; set; }
        public float Humid { get; set; }
        public DateTime Timestamp { get; set; }

        public Status(float temp, float humid)
        {
            Temp = temp;
            Humid = humid;
        }

        public Status() { }

        public override string ToString()
        {
            return $"Id: {Id}, Temp: {Temp}, Humid: {Humid}, Timestamp: {Timestamp}";
        }
    }
}
