using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terra.Models
{
    public class Settings
    {
        private int _id;
        private int _light;
        private float _temp;
        private float _humid;

        public Settings(int id, int light, float temp, float humid)
        {
            _id = id;
            _light = light;
            _temp = temp;
            _humid = humid;
        }

        public Settings() {}

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int Light
        {
            get { return _light; }
            set { _light = value; }
        }

        public float Temp
        {
            get { return _temp; }
            set { _temp = value; }
        }

        public float Humid
        {
            get { return _humid; }
            set { _humid = value; }
        }

        public override string ToString()
        {
            return $"{Id}, {Light}, {Temp}, {Humid}";

        }
    }
}
