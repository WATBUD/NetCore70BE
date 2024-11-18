using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using NetCore60.Models;
using NetCore60.Services;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using NetCore60.Utilities;
using NetCore60.DTO;

namespace NetCore60.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "G_User")]
    public class UserController : ControllerBase
    {
        private readonly RNDatingService _databaseService;
        public UserController(RNDatingService databaseService) // Constructor
        {
            _databaseService = databaseService;

        }
        //ActionResult<User> Can generate schema UI
        /// <summary> 
        ///     創造使用者        
        /// </summary>
        [HttpPost("CreateUser")]
        public IActionResult Create([Required] string _account, [Required] string _password, [Required] string _email)
        {
            string newUserId = _databaseService.InsertUserAccount(_account, _password, _email);
            return Ok(newUserId);
            //return CreatedAtAction(nameof(GetUserById), new { id = newUserId }, item);
        }

        /// <summary> 
        /// 檢查用户基本訊息
        /// </summary>
        /// <returns></returns> 
        [Authorize]
        [HttpGet("CheckUserBasicInformation")]
        public IActionResult checkUserBasicInformation()
        {
            string jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            int getUserIdFromToken = JsonWebToken.TryGetUserIdFromJwtToken(jwtToken);

            var user = _databaseService.checkUserBasicInformation(getUserIdFromToken);

            if (user == null)
            {
                //return NotFound();
                return Ok("用戶ID不存在");
            }

            return Ok(user);
        }

        ///// <summary> 
        /////  登入使用者帳號
        ///// </summary>
        ///// <param name="formModel"></param>
        ///// <returns></returns> 
        ///// <remarks>注意事項</remarks> 
        ///// 
        //[HttpPost("Login")]
        //public IActionResult Login([FromBody] UserDTO formModel)
        //{
        //    //int user = _databaseService.GetLoginUserId(formModel);
        //    //if (user == -1)
        //    //{
        //    //    return Ok("Please check your username and password.");
                
        //    //}
        //    //else{
        //    //    var token = JsonWebToken.GenerateJwtToken(user);
        //    //    return Ok(new { token });
        //    //}
        //}
        /// <summary> 
        ///  test_jwt_token
        /// </summary>
        [HttpPost("test_jwt_token")]
        [Obsolete]
        public IActionResult GetJWTToken()
        {
            var token = JsonWebToken.GenerateJwtToken(10);
            return Ok(new { token });
        }

        /// <summary> 
        ///  驗證Token
        /// </summary>
        [HttpPost("ValidateToken")]
        public IActionResult ValidateToken([Required] string token)
        {
            var user = TokenStore.FindUserIdByToken(token);
            if (user == null)
            {
                // Return a response indicating the token does not exist
                return Ok("token does not exist");
            }
            else
            {
                // Return the user ID if the token exists
                return Ok(user);
            }
        }


        /// <summary> 
        ///     更新使用者密碼
        /// </summary>
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
        /// 上傳圖片到伺服器
        /// </summary>
        /// <remarks>限制最大上傳大小為1MB</remarks>
        /// <param name="file">上傳的圖片文件</param>
        /// <returns>
        /// 200 - OK，上傳成功
        /// 400 - Not Found，未找到
        /// </returns>
        [HttpPost("UploadImageFileToServer")]
        public IActionResult UploadImageFileToServer(IFormFile file)
        {
            string resultMessage= FileHelper.UploadImageFileToServer(file);
            return Ok(resultMessage);
        }



        [HttpGet("GetAssignIPInfo")]
        public async Task<IActionResult> GetAssignIPInfoAsync([Required]string ipAddress)
        {
            try
            {
                var httpClientService = new HttpClientService();
                string response;
                response = await httpClientService.GetNordVPNDataAsync(ipAddress);      
                if (response != null)
                {
                    //string combinedData = $"User IP Address: {ipAddress}\n响应数据：\n{response}";
                    return Content(response);
                }
                else
                {
                    return Content("未能获取响应数据。");
                }
            }
            catch (Exception ex)
            {
                return Content($"发生异常：{ex.Message}");
            }

        }

        [HttpGet("GetClientIP")]
        public async Task<IActionResult> GetClientIPAsync()
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "IP Address not available";
            //return Content($"User IP Address: {ipAddress}");
            //string ipAddress = "36.224.158.30";
            try
            {

                var httpClientService = new HttpClientService();
                string response;

                if (ipAddress == "::1" || ipAddress == "127.0.0.1")
                {
                    var mylocalip = await httpClientService.GetLocalPublicIpAddressAsync();
                    response = await httpClientService.GetNordVPNDataAsync(mylocalip);
                }

                else
                {
                    response = await httpClientService.GetNordVPNDataAsync(ipAddress);

                }
                if (response != null)
                {
                    //string combinedData = $"User IP Address: {ipAddress}\n响应数据：\n{response}";
                    return Content(response);
                }
                else
                {
                    return Content("未能获取响应数据。");
                }
            }
            catch (Exception ex)
            {
                return Content($"发生异常：{ex.Message}");
            }

        }

        /// <summary> 
        /// 取得tag群組表
        /// </summary>
        /// <response code="200">OK</response> 
        /// <response code="400">Not found</response> d
        /// <returns></returns> 
        /// <remarks>注意事項</remarks> 
        /// 
        [HttpGet("GetTagGroupDetails")]
        public IActionResult GetTagGroupDetails()
        {
            var resultList = _databaseService.GetTagGroupDetails();
            return Ok(resultList);
        }

        /// <summary> 
        /// Get Api call record table
        /// </summary>
        /// <response code="200">OK</response> 
        /// <response code="400">Not found</response> d
        /// <returns></returns> 
        /// <remarks>注意事項</remarks> 
        /// 
        [HttpGet("GetRequestLogs")]
        public IActionResult GetRequestLogs()
        {
            var resultList = _databaseService.GetRequestLogs();
            return Ok(resultList);
        }


    }
}
