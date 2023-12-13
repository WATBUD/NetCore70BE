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
        public IActionResult GetUserById()
        {
            string jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
            int getUserIdFromToken = JsonWebTokenService.TryGetUserIdFromJwtToken(jwtToken);

            var user = _databaseService.GetUserById(getUserIdFromToken);

            if (user == null)
            {
                //return NotFound();
                return Ok("用戶ID不存在");
            }

            return Ok(user);
        }

        /// <summary> 
        ///  登入使用者帳號
        /// </summary>
        /// <param name="formModel"></param>
        /// <returns></returns> 
        /// <remarks>注意事項</remarks> 
        /// 
        [HttpPost("Login")]
        public IActionResult Login([FromForm] LoginFormModel formModel)
        {
            int user = _databaseService.GetLoginUserId(formModel);
            if (user == -1)
            {
                return Ok("Please check your username and password.");
                
            }
            else{
                var token = JsonWebTokenService.GenerateJwtToken(user);
                return Ok(new { token });
            }
        }
        ///// <summary> 
        /////  GetJWTToken
        ///// </summary>
        //[HttpPost("GetJWTToken")]
        //public IActionResult GetJWTToken()
        //{
        //    var token = JsonWebTokenService.GenerateJwtToken(10);
        //    return Ok(new { token });
        //}

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
        ///  獲取用户詳細訊息
        /// </summary>
        /// <param name="id">Member Id</param> 
        /// <response code="200">OK</response> 
        /// <response code="400">Not found</response> 
        /// <returns></returns> 
        /// <remarks>獲取用户詳細訊息</remarks> 
        /// 
        [Authorize]
        [HttpGet("GetUserDetail/{id}")]
        public IActionResult GetUserDetail(int id)
        {
            var user = _databaseService.GetUserDetail(id);

            if (user == null)
            {
                //return NotFound();
                return Ok("用戶名不存在");
            }

            return Ok(user);
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
        /// 更新使用者詳細訊息
        /// </summary>
        /// <remarks>
        /// Object JSONstringify=>: "{\\"key\\":\\"value\\"}",<br/>
        /// Array JSONstringify=>: "[\"1\",\"2\"]", <br/>
        /// Example request:
        ///"relationship_status" ENUM=>: 'Single','Married','Divorced','Other'.<br/>
        ///"looking_for": ENUM=>'Friendship','Dating','Long-term Relationship','Other'.<br/>
        ///"userHasTag":Array JSONstringify,<br/>
        ///"privacySettings" Array JSONstringify,<br/>
        ///"social_links": Array JSONstringify,<br/>
        /// </remarks>
        /// <response code="200">OK</response> 
        /// <response code="400">Not found</response> 
        /// <param name="_VUsersDetailDTO.UserId">The ID of the user to update.</param>
        /// <returns>Returns a response indicating the result of the Detail update.</returns>
        //[HttpPost("UpdateUserDetail")]
        ////[SwaggerResponse(200, "Success")]
        ////[SwaggerResponse(400, "Bad Request")]
        //[SwaggerResponse(500, "Internal Server Error")]
        //public IActionResult UpdateUserDetail(VUsersDetailDTO _VUsersDetailDTO)
        //{
            
        //    var callbackResult = _databaseService.UpdateUserDetail(_VUsersDetailDTO);
        //    if (callbackResult?.GetType() == typeof(string))
        //    {
        //        return Ok(callbackResult);// 非成功錯誤訊息

        //    }
        //    else
        //    {
        //        var successMessage = "更新使用者詳細訊息成功";
        //        var result = new OkObjectResult(new { Message = successMessage, Data = callbackResult });
        //        return result;
        //    }

        //}






        [HttpPost("UpdateUserDetailByFormData")]
        public IActionResult UploadMixedFormData([FromForm] VUsersDetailDTO _VUsersDetailDTO)
        {
            try
            {
                // 使用 System.Text.Json 或 Newtonsoft.Json 将 json 字符串反序列化为对象
                // 这里假设您使用 System.Text.Json
                //var userDetail = System.Text.Json.JsonSerializer.Deserialize<Object>(_VUsersDetailDTO);
                //JsonDocument jsonDocument = JsonDocument.Parse(_VUsersDetailDTO);
                var callbackResult = _databaseService.UpdateUserDetail(_VUsersDetailDTO);
       
                if (callbackResult?.GetType() == typeof(string))
                {
                    return Ok(callbackResult);// 非成功錯誤訊息
                }
                else
                {
                    var successMessage = "更新使用者詳細訊息成功";
                    //return Ok("User details updated successfully");
                    var result = new OkObjectResult(new { Message = successMessage, Data = callbackResult });
                    return result;
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
                //return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }

        ///// <summary>
        ///// 更新使用者單一詳細訊息
        ///// </summary>
        ///// <remarks>
        ///// Object JSONstringify=>: "{\\"key\\":\\"value\\"}",<br/>
        ///// Array JSONstringify=>: "[\"1\",\"2\"]", <br/>
        ///// Example request:
        /////"relationship_status" ENUM=>: 'Single','Married','Divorced','Other'.<br/>
        /////"looking_for": ENUM=>'Friendship','Dating','Long-term Relationship','Other'.<br/>
        /////"userHasTag":Array JSONstringify,<br/>
        /////"privacySettings" Array JSONstringify,<br/>
        /////"social_links": Array JSONstringify,<br/>
        /////"{\\"password\\":\\"557788\\"}"
        ///// </remarks>
        ///// <response code="200">OK</response> 
        ///// <response code="400">Not found</response> 
        ///// <param name="UserId">The ID of the user to update.</param>
        ///// <param name="_VUsersDetailJsonString"></param>
        ///// <returns>Returns a response indicating the result of the Detail update.</returns>
        //[HttpPost("UpdateUserSingleDetails")]
        //[SwaggerResponse(500, "Internal Server Error")]
        //public IActionResult UpdateUserSingleDetails([Required] int UserId, [FromBody] string _VUsersDetailJsonString)
        //{
        //    try
        //    {
        //        // 使用 System.Text.Json 或 Newtonsoft.Json 将 json 字符串反序列化为对象
        //        // 这里假设您使用 System.Text.Json
        //        //var userDetail = System.Text.Json.JsonSerializer.Deserialize<Object>(_VUsersDetailDTO);
        //        //JsonDocument jsonDocument = JsonDocument.Parse(_VUsersDetailDTO);
        //        if (_VUsersDetailJsonString == null)
        //        {
        //            return Ok("請指定要更新的欄位");
        //        }


        //        var callbackResult = _databaseService.UpdateUserSingleDetails(UserId, _VUsersDetailJsonString);
        //        if (callbackResult == null)
        //        {
        //            return Ok("用戶ID不存在");
        //        }
        //        else if (callbackResult?.GetType() == typeof(string))
        //        {
        //            return Ok(callbackResult);// 非成功錯誤訊息
        //        }
        //        else
        //        {
        //            var successMessage = "更新使用者詳細訊息成功";
        //            var result = new OkObjectResult(new { Message = successMessage, Data = callbackResult });
        //            return result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Error: {ex.Message}");
        //    }

        //}

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
            string resultMessage= SystemService.UploadImageFileToServer(file);
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
