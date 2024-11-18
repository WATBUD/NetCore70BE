using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCore60.DTO
{
    /// <summary>
    /// 用戶登入信息
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// 用戶帳號
        /// </summary>
        public string? UserAccount { get; set; }

        /// <summary>
        /// 用戶密碼
        /// </summary>
        public string? Password { get; set; }
    }


}