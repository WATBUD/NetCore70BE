using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace NetCore60.Models;
public partial class LoginFormModel
{
    [Required]
    public string Account { get; set; } = null!;
    [Required]
    public string Password { get; set; } = null!;
}
