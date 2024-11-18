using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCore60.DTO
{
    /// <summary>
    /// �Τ�n�J�H��
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// �Τ�b��
        /// </summary>
        public string? UserAccount { get; set; }

        /// <summary>
        /// �Τ�K�X
        /// </summary>
        public string? Password { get; set; }
    }


}