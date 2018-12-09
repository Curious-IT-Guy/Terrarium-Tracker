using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
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
                var cmd = new SqlCommand(@"

                    SELECT * FROM Animals " +
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

                    animal.DaySettings = daySetting;
                    animal.NightSettings = nightSetting;

                    resultList.Add(animal);

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

                var cmd = new SqlCommand(@"

                    BEGIN TRANSACTION;

                    UPDATE Animals SET is_active = IIF(@is_active = 1, 0, 1) WHERE is_active = 1;
                    
                    UPDATE Animals 
                    SET Animals.title = @title, Animals.is_active = @is_active
                    WHERE Animals.id = @id;

                    UPDATE Day_settings
                    SET
                    Day_settings.temp = @day_settings_temp,
                    Day_settings.humid = @day_settings_humid,
                    Day_settings.light = @day_settings_light,
                    Day_settings.time = @day_settings_time
                    FROM Day_settings, Animals
                    WHERE  Animals.fk_day_id = Day_settings.id
                    AND Animals.id = @id;

                    UPDATE Night_settings
                    SET
                    Night_settings.temp = @night_settings_temp,
                    Night_settings.humid = @night_settings_humid,
                    Night_settings.light = @night_settings_light,
                    Night_settings.time = @night_settings_time
                    FROM Night_settings, Animals
                    WHERE  Animals.fk_night_id = Night_settings.id
                    AND Animals.id = @id;

                    COMMIT;

                ", conn);


                cmd.Parameters.AddWithValue("@title", value.Title);
                cmd.Parameters.AddWithValue("@is_active", value.IsActive);

                cmd.Parameters.AddWithValue("@day_settings_temp", value.DaySettings.Temp);
                cmd.Parameters.AddWithValue("@day_settings_humid", value.DaySettings.Humid);
                cmd.Parameters.AddWithValue("@day_settings_light", value.DaySettings.Light);
                cmd.Parameters.AddWithValue("@day_settings_time", value.DaySettings.Time);

                cmd.Parameters.AddWithValue("@night_settings_temp", value.NightSettings.Temp);
                cmd.Parameters.AddWithValue("@night_settings_humid", value.NightSettings.Humid);
                cmd.Parameters.AddWithValue("@night_settings_light", value.NightSettings.Light);
                cmd.Parameters.AddWithValue("@night_settings_time", value.NightSettings.Time);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }


        [HttpPost]
        public void Post(Animal value)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                var cmd = new SqlCommand(@"
                    
                    BEGIN TRANSACTION;

                    UPDATE Animals SET is_active = IIF(@is_active = 1, 0, 1) WHERE is_active = 1;

                    INSERT Day_settings (temp, humid, light, time)
                    VALUES (@day_settings_temp, @day_settings_humid, @day_settings_light, @day_settings_time);

                    declare @fk_day_id int = scope_identity();
                    
                    INSERT Night_settings (temp, humid, light, time)
                    VALUES (@night_settings_temp, @night_settings_humid, @night_settings_light, @night_settings_time);

                    declare @fk_night_id int = scope_identity();

                    INSERT Animals (title, is_active, fk_day_id, fk_night_id) 
                    VALUES (@title, @is_active, @fk_day_id, @fk_night_id);

                    COMMIT;

                ", conn);


                cmd.Parameters.AddWithValue("@title", value.Title);
                cmd.Parameters.AddWithValue("@is_active", value.IsActive);

                cmd.Parameters.AddWithValue("@day_settings_temp", value.DaySettings.Temp);
                cmd.Parameters.AddWithValue("@day_settings_humid", value.DaySettings.Humid);
                cmd.Parameters.AddWithValue("@day_settings_light", value.DaySettings.Light);
                cmd.Parameters.AddWithValue("@day_settings_time", value.DaySettings.Time);

                cmd.Parameters.AddWithValue("@night_settings_temp", value.NightSettings.Temp);
                cmd.Parameters.AddWithValue("@night_settings_humid", value.NightSettings.Humid);
                cmd.Parameters.AddWithValue("@night_settings_light", value.NightSettings.Light);
                cmd.Parameters.AddWithValue("@night_settings_time", value.NightSettings.Time);

                cmd.ExecuteNonQuery();
            }
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();

                var cmd = new SqlCommand(@"DELETE FROM Animals WHERE Animals.id = @id;", conn);

                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }
        }

    }
}
