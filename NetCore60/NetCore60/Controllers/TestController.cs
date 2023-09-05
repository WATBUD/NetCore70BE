using Microsoft.AspNetCore.Mvc;
using NetCore60.Models;
using NetCore60.Services;
using System.Collections.Generic;
using System.Linq;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "G_Test")]
    public class TestController : ControllerBase
    {
        //private readonly IDatabaseService _databaseService;

        //public TestController(IDatabaseService databaseService)//Constructor
        //{
        //    _databaseService = databaseService;
        //}
        //[HttpGet("testconnection")]
        //public ActionResult<string> TestConnection()
        //{
        //    string connectionStatus = _databaseService.Connect();
        //    return connectionStatus;
        //}
        private readonly RNDatingService _databaseService;

        public TestController(RNDatingService databaseService) // Constructor
        {
            _databaseService = databaseService;
        }


        [HttpGet("testconnection")]
        public ActionResult<string> TestConnection()
        {
            string connectionStatus = _databaseService.testConnectionDatabase();
            return connectionStatus;
        }

    }
}
