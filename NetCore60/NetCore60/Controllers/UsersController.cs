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
using System.Security.Claims;
using static NetCore60.DTO.ResponseDTO;
using Microsoft.EntityFrameworkCore;
namespace NetCore60.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "G_User")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly RNDatingService _databaseService;
        private readonly UsersService _usersService;

        public UsersController(ApplicationDbContext context, RNDatingService databaseService, UsersService usersService) // Constructor
        {
            _databaseService = databaseService;
            _usersService = usersService;
            _context = context;

        }
        ///// <summary> 
        /////     創造使用者        
        ///// </summary>
        //[HttpPost("CreateUser")]
        //public IActionResult Create([Required] string _account, [Required] string _password, [Required] string _email)
        //{
        //    //string newUserId = _databaseService.InsertUserAccount(_account, _password, _email);
        //    return Ok("");
        //    //return CreatedAtAction(nameof(GetUserById), new { id = newUserId }, item);
        //}

        /// <summary> 
        /// 檢查用户基本訊息
        /// </summary>
        /// <returns></returns> 
        [Authorize]
        [HttpGet("GetUserInfo")]
        public async Task<ResponseDTO> GetUserInfo()
        {
            string? userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Find the target user
            var user = await _context.Users
                .Where(u => u.UserId.ToString() == userId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                // If user does not exist, return an error response
                return ErrorResponse("Error", new { message = "User does not exist." });
            }


            return SuccessResponse(new { UserInfo = user });
        }




        ///// <summary> 
        ///// 登入使用者帳號
        ///// </summary>
        ///// <param name="model">用戶的登入信息</param>
        ///// <returns>回應 DTO</returns> 
        ///// <remarks>注意事項：此 API 用於登入，請確保傳入有效的帳號與密碼。</remarks> 
        [HttpPost("login")]
        public async Task<ResponseDTO> Login([FromBody] LoginDTO model)
        {
            return await _usersService.Login(model);
        }



        ///// <summary> 
        /////  test_jwt_token
        ///// </summary>
        //[HttpPost("test_jwt_token")]
        //[Obsolete]
        //public IActionResult GetJWTToken()
        //{
        //    var token = JsonWebToken.GenerateJwtToken(10);
        //    return Ok(new { token });
        //}

        ///// <summary> 
        /////  驗證Token
        ///// </summary>
        //[HttpPost("ValidateToken")]
        //public IActionResult ValidateToken([Required] string token)
        //{
        //    var user = TokenStore.FindUserIdByToken(token);
        //    if (user == null)
        //    {
        //        // Return a response indicating the token does not exist
        //        return Ok("token does not exist");
        //    }
        //    else
        //    {
        //        // Return the user ID if the token exists
        //        return Ok(user);
        //    }
        //}


        //[Authorize]
        //[HttpPut("ModifyPassword")]
        //public async Task<ResponseDTO> ModifyPassword([FromBody] ChangePasswordDTO model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var errorMessages = ModelState.Values
        //                                      .SelectMany(v => v.Errors)
        //                                      .Select(e => e.ErrorMessage)
        //                                      .ToList();
        //        return ErrorResponse("Error", new { message = string.Join(", ", errorMessages) });
        //    }
        //    var userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (int.TryParse(userId, out int parsedUserId))
        //    {
        //        model.UserId = parsedUserId;
        //    }
        //    else
        //    {
        //        return ErrorResponse("Error", new { message = "Invalid UserId." });
        //    }
        //    return await _usersService.ModifyPassword(model);
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
