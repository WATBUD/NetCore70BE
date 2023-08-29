using Microsoft.AspNetCore.Mvc;
using NetCore60.Models;
using NetCore60.Services;
using System.Collections.Generic;
using System.Linq;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
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
        private readonly DatabaseService _databaseService;

        public UserController(DatabaseService databaseService) // Constructor
        {
            _databaseService = databaseService;
        }

        [HttpGet("testconnection")]
        public ActionResult<string> TestConnection()
        {
            _databaseService.testDbContext();
            string connectionStatus = _databaseService.Connect();
            return connectionStatus;
        }

        [HttpPost]
        public ActionResult<User> Create(string _username, string _password)
        {
            //item.Id = _nextId++;
            //_items.Add(item);
            string newUserId = _databaseService.InsertUserAccount(_username, _password);

            return Ok(newUserId);
            //return CreatedAtAction(nameof(GetUserById), new { id = newUserId }, item);
        }
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            // 通过 _userService 获取用户信息
            var user = _databaseService.GetUserById(id);

            if (user == null)
            {
                //return NotFound();
                return Ok("用戶ID不存在");
            }

            return Ok(user);
        }

    }
}
