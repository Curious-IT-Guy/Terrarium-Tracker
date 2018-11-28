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
        private string _timeAPI = @"http://worldclockapi.com/api/json/utc/now";

        // GET api/settings/day
        [HttpGet("day")]
        public Settings GetDay()
        {
            return null;
        }

        // GET api/settings/day
        public Settings GetNight()
        {
            return null;
        }

        // PUT api/settings/day
        [HttpPut("day")]
        public void PutDay(int id, [FromBody] string value)
        {
            
        }

        // PUT api/settings/night
        [HttpPut("night")]
        public void PutNight(int id, [FromBody] string value)
        {

        }
    }
}
