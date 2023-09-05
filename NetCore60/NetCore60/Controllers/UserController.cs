using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using NetCore60.Models;
using NetCore60.Services;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "G_User")]
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
        private readonly RNDatingService _databaseService;

        public UserController(RNDatingService databaseService) // Constructor
        {
            _databaseService = databaseService;
        }
        [HttpPost("CreateUser")]
        public ActionResult<User> Create([Required] string _account, [Required] string _password, [Required] string _email)
        {
            string newUserId = _databaseService.InsertUserAccount(_account, _password, _email);
            return Ok(newUserId);
            //return CreatedAtAction(nameof(GetUserById), new { id = newUserId }, item);
        }

        /// <summary> 
        ///     获取用户信息
        /// </summary>
        /// <param name="id">Member Id</param> 
        /// <response code="200">OK</response> 
        /// <response code="400">Not found</response> 
        /// <returns></returns> 
        /// <remarks>注意事項</remarks> 
        /// 
        [HttpGet("CheckByID/{id}")]
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


        /// <summary> 
        ///     获取用户信息
        /// </summary>
        /// <param name="id">Member Id</param> 
        /// <response code="200">OK</response> 
        /// <response code="400">Not found</response> 
        /// <returns></returns> 
        /// <remarks>注意事項</remarks> 
        /// 
        [HttpGet("GetUserDetail/{id}")]
        public IActionResult GetUserDetail(int id)
        {
            // 通过 _userService 获取用户信息
            var user = _databaseService.GetUserDetail(id);

            if (user == null)
            {
                //return NotFound();
                return Ok("用戶名不存在");
            }

            return Ok(user);
        }

        [HttpPut("UpdateUserPassWord")]
        public IActionResult UpdateUserPassWord([Required] int _user_id, [Required] string _password)
        {
            var user = _databaseService.UpdateUserPassword(_user_id, _password);
            if (user == null)
            {
                //return NotFound();
                return Ok("用戶ID不存在");
            }
            // 构建包含更新成功消息的 OkObjectResult
            var successMessage = "密码更新成功";
            var result = new OkObjectResult(new { Message = successMessage, Data = user });
            return result;
        }

        /// <summary>
        /// Update user's Detail.
        /// </summary>
        /// <remarks>
        /// Example request:
        ///"relationship_status" ENUM=>: 'Single','Married','Divorced','Other'.<br/>
        ///"looking_for": ENUM=>'Friendship','Dating','Long-term Relationship','Other'.<br/>
        ///"privacySettings" JSON=>: '{"key": "value"}'.<br/>
        ///"social_links": JSON=>'{"key": "value"}'.<br/>
        /// </remarks>
        /// <response code="200">OK</response> 
        /// <response code="400">Not found</response> 
        /// <param name="_VUsersDetail.UserId">The ID of the user to update.</param>
        /// <returns>Returns a response indicating the result of the Detail update.</returns>
        [HttpPost("UpdateUserDetail")]
        //[SwaggerResponse(200, "Success")]
        //[SwaggerResponse(400, "Bad Request")]
        [SwaggerResponse(500, "Internal Server Error")]
        public IActionResult UpdateUserDetail(VUsersDetail _VUsersDetail)
        {
            var callbackResult = _databaseService.UpdateUserDetail(_VUsersDetail);
            if (callbackResult==null)
            {
                //var user = _databaseService.UpdateUserDetail((string)_VUsersDetail);
                return Ok("用戶ID不存在");//
            }
            else if (callbackResult?.GetType() == typeof(string))
            {
                //var user = _databaseService.UpdateUserDetail((string)_VUsersDetail);
                return Ok(callbackResult);// "用戶ID不存在"
            }
            // 构建包含更新成功消息的 OkObjectResult
            else
            {
                var successMessage = "密码更新成功";
                var result = new OkObjectResult(new { Message = successMessage, Data = callbackResult });
                return result;
            }

        }


    }
}
