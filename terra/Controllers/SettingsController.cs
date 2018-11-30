using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using terra.Models;

namespace terra.Controllers
{
    [Route("api/settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private static dynamic _config =
            JsonConvert.DeserializeObject(System.IO.File.ReadAllText(@"config/appsettings.json"));

        private string _connection = _config.ConnectionStrings.DefaultConnection;
        //private string _timeAPI = @"http://worldclockapi.com/api/json/utc/now";

        // GET api/settings/day
        [HttpGet("day")]
        public Settings GetDay()
        {
            using (var conn = new SqlConnection(_connection))
            {
                var cmd = new SqlCommand($"SELECT * FROM day_settings", conn);
                conn.Open();
                SqlDataReader result = cmd.ExecuteReader();
                Settings daySettings = new Settings();
                while (result.Read())
                {
                    daySettings.Id = Int32.Parse(result[0].ToString());
                    daySettings.Light = Int32.Parse(result[1].ToString());
                    daySettings.Temp = float.Parse(result[2].ToString());
                    daySettings.Humid = float.Parse(result[3].ToString());
                    daySettings.Time = (TimeSpan)result[4];
                }
                return daySettings;
            }
        }

        // GET api/settings/night
        [HttpGet("night")]
        public Settings GetNight()
        {
            using (var conn = new SqlConnection(_connection))
            {
                var cmd = new SqlCommand($"SELECT * FROM night_settings", conn);
                conn.Open();
                SqlDataReader result = cmd.ExecuteReader();
                Settings nightSettings = new Settings();
                while (result.Read())
                {
                    nightSettings.Id = Int32.Parse(result[0].ToString());
                    nightSettings.Light = Int32.Parse(result[1].ToString());
                    nightSettings.Temp = float.Parse(result[2].ToString());
                    nightSettings.Humid = float.Parse(result[3].ToString());
                    nightSettings.Time = (TimeSpan)result[4];
                }
                return nightSettings;
            }
        }

        // PUT api/settings/day
        [HttpPut("day")]
        public void PutDay([FromBody] Settings settings)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();
                var cmd = new SqlCommand($"UPDATE day_settings SET id = '{settings.Id}', light = '{settings.Light}', temp = '{settings.Temp}', humid = '{settings.Humid}'," +
                                         $"time = '{settings.Time}' WHERE id = {settings.Id}", conn);
                cmd.ExecuteNonQuery();
            }
        }

        // PUT api/settings/night
        [HttpPut("night")]
        public void PutNight([FromBody] Settings settings)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();
                var cmd = new SqlCommand($"UPDATE night_settings SET id = '{settings.Id}', light = '{settings.Light}', temp = '{settings.Temp}', humid = '{settings.Humid}'," +
                                         $"time = '{settings.Time}' WHERE id = {settings.Id}", conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
