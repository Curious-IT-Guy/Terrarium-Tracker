using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using terra.Models;

namespace terra.Controllers
{
    [Route("api/status")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private static dynamic _config =
            JsonConvert.DeserializeObject(System.IO.File.ReadAllText(@"config/appsettings.json"));

        private string _connection = _config.ConnectionStrings.DefaultConnection;
        private string _timeAPI = "http://worldclockapi.com/api/json/utc/now";

        // GET: api/status
        [HttpGet]
        public IEnumerable<Status> Get()
        {
            using (var conn = new SqlConnection(_connection))
            {
                var cmd = new SqlCommand($"SELECT TOP 10 * FROM Status ORDER BY timestamp DESC", conn);
                conn.Open();
                var result = cmd.ExecuteReader();
                var resultList = new List<Status>();
                while (result.Read())
                {
                    var status = new Status();
                    status.Id = Int32.Parse(result[0].ToString());
                    status.Temp = float.Parse(result[1].ToString());
                    status.Humid = float.Parse(result[2].ToString());
                    status.Timestamp = DateTime.Parse(result[3].ToString());
                    resultList.Add(status);
                }
                return resultList;
            }
        }

        // POST: api/status
        [HttpPost]
        public void Post([FromBody] Status status)
        {
            DateTime currentTime;
            using (HttpClient client = new HttpClient())
            {
                dynamic response = JsonConvert.DeserializeObject(client.GetStringAsync(_timeAPI).Result);
                currentTime = response.currentDateTime;
            }
            string currentTimeFormatted = currentTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

            using (var conn = new SqlConnection(_connection))
            {
                conn.Open();
                var cmd = new SqlCommand($"INSERT INTO Status VALUES('{status.Temp}', '{status.Humid}', '{currentTimeFormatted}')", conn);
                cmd.ExecuteNonQuery();

                var deleteOldRecords = new SqlCommand("DELETE FROM Status WHERE id IN (SELECT id FROM Status ORDER BY timestamp DESC OFFSET 10 ROWS)", conn);
                deleteOldRecords.ExecuteNonQuery();
            }
        }
    }
}
