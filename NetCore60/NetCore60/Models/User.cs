using System;
using System.Collections.Generic;

namespace NetCore60.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Account { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual UserDetail? UserDetail { get; set; }
}
