using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terra.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public int FkDayId { get; set; }
        public int FkNightId { get; set; }

        //public List<Settings> Settings { get; set; }

        public Settings DaySettings { get; set; }
        public Settings Nightsettings { get; set; }

        public Animal() { }
    }
}
