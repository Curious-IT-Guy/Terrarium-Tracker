using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terra.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public bool Light { get; set; }
        public float Temp { get; set; }
        public float Humid { get; set; }
        public TimeSpan Time { get; set; }

        public Settings() {}
    }
}
