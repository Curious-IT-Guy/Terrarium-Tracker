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

        [HttpGet]
        public IEnumerable<object> GetAnimals()
        {
            using (var conn = new SqlConnection(_connection))
            {
                var cmd = new SqlCommand("SELECT * FROM Animals " +
                                         "LEFT JOIN Day_settings ON Animals.fk_day_id = Day_settings.id " +
                                         "LEFT JOIN Night_settings ON Animals.fk_night_id = Night_settings.id " +
                                         "ORDER BY is_active DESC", conn);
                conn.Open();
                var result = cmd.ExecuteReader();
                var resultList = new List<object>();
                while (result.Read())
                {
                    var animal = new Animal();
                    animal.Id = int.Parse(result[0].ToString());
                    animal.Title = result[1].ToString();
                    animal.IsActive = bool.Parse(result[2].ToString());
                    animal.FkDayId = int.Parse(result[3].ToString());
                    animal.FkNightId = int.Parse(result[4].ToString());

                    var daySetting = new Settings();
                    daySetting.Id = int.Parse(result[5].ToString());
                    daySetting.Temp = float.Parse(result[6].ToString());
                    daySetting.Humid = float.Parse(result[7].ToString());
                    daySetting.Light = bool.Parse(result[8].ToString());
                    daySetting.Time = (TimeSpan)result[9];

                    var nightSetting = new Settings();
                    nightSetting.Id = int.Parse(result[10].ToString());
                    nightSetting.Temp = float.Parse(result[11].ToString());
                    nightSetting.Humid = float.Parse(result[12].ToString());
                    nightSetting.Light = bool.Parse(result[13].ToString());
                    nightSetting.Time = (TimeSpan)result[14];

                    resultList.Add(animal);
                    resultList.Add(daySetting);
                    resultList.Add(nightSetting);
                }
                return resultList;
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Animal value)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();
                var cmd = new SqlCommand(
                    $"UPDATE Animals " +
                    $"SET " +
                    $"Animals.title = @title, " +
                    $"Animals.is_active = @is_active, " +

                    $"Day_settings.temp = @day_settings_temp, " +
                    $"Day_settings.humid = @day_settings_humid, " +
                    $"Day_settings.light = @day_settings_light, " +
                    $"Day_settings.time = @day_settings_time, " +

                    $"Night_settings.temp = @night_settings_temp, " +
                    $"Night_settings.humid = @night_settings_humid, " +
                    $"Night_settings.light = @night_settings_light, " +
                    $"Night_settings.time = @night_settings_time " +

                    $"FROM Animals " +
                    $"LEFT JOIN Day_settings ON Animals.fk_day_id = Day_settings.id " +
                    $"LEFT JOIN Night_settings ON Animals.fk_night_id = Night_settings.id " +
                    $"WHERE Animals.id = @id", conn);

                cmd.Parameters.AddWithValue("@title", value.Title);
                cmd.Parameters.AddWithValue("@is_active", value.IsActive);

                cmd.Parameters.AddWithValue("@day_settings_temp", value.DaySettings.Temp);
                cmd.Parameters.AddWithValue("@day_settings_humid", value.DaySettings.Humid);
                cmd.Parameters.AddWithValue("@day_settings_light", value.DaySettings.Light);
                cmd.Parameters.AddWithValue("@day_settings_time", value.DaySettings.Time);

                cmd.Parameters.AddWithValue("@night_settings_temp", value.Nightsettings.Temp);
                cmd.Parameters.AddWithValue("@night_settings_humid", value.Nightsettings.Humid);
                cmd.Parameters.AddWithValue("@night_settings_light", value.Nightsettings.Light);
                cmd.Parameters.AddWithValue("@night_settings_time", value.Nightsettings.Time);

                cmd.Parameters.AddWithValue("@id", id);

               cmd.ExecuteNonQuery();
            }
        }


/*        // GET api/settings/day
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
        }*/

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
