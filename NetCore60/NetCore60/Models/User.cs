using System;
using System.Collections.Generic;

namespace NetCore60.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool? IsBanned { get; set; }

    public virtual UserStatus? UserStatus { get; set; }
}
