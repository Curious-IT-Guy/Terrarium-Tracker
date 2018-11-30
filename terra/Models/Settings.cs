using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terra.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public int Light { get; set; }
        public float Temp { get; set; }
        public float Humid { get; set; }
        public TimeSpan Time { get; set; }


        public Settings(int id, int light, float temp, float humid, TimeSpan time)
        {
            Id = id;
            Light = light;
            Temp = temp;
            Humid = humid;
            Time = time;
        }

        public Settings() {}


        public override string ToString()
        {
            return $"Id: {Id}, Light: {Light}, Temp: {Temp}, Humid: {Humid}, Time: {Time}";
        }
    }
}
