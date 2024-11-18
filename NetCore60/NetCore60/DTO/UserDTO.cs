using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCore60.DTO
{
    public class UserDTO
    {
        public string UserAccount { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "PlaceId is required.")]
        public int? PlaceId { get; set; }
        [Required(ErrorMessage = "GroupId is required.")]
        public int? GroupId { get; set; }
    }

    public class UpdateUserInfoDTO
    {
        public string UserName { get; set; }
        public int? PlaceId { get; set; }
        public int? GroupId { get; set; }
    }
    public class ChangePasswordDTO
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "OldPassword is required.")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "NewPassword is required.")]
        public string NewPassword { get; set; }
    }

    public class LoginDTO
    {
        public string UserAccount { get; set; }
        public string Password { get; set; }
    }

    public class UserGroupDTO
    {
        //[Required(ErrorMessage = "GroupId is required.")]
        //public int? GroupId { get; set; } 
        [Required(ErrorMessage = "GroupName is required.")]
        public string GroupName { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "PermissionIds is required.")]
        public List<int> PermissionIds { get; set; }
    }

}