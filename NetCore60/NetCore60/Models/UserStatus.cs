using System;
using System.Collections.Generic;

namespace NetCore60.Models;

public partial class UserStatus
{
    public int UserId { get; set; }

    public bool? IsBanned { get; set; }

    public virtual User User { get; set; } = null!;
}
