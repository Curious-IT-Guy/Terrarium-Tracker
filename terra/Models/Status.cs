using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace terra.Models
{
    public class Status
    {
        public int Id { get; set; }
        public bool IsDay { get; set; }
        public float Temp { get; set; }
        public float Humid { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Light { get; set; }
        public int FkAnimalId { get; set; }

        public Status() { }
    }
}
