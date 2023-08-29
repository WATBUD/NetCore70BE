using System;
using System.Collections.Generic;

namespace NetCore60.Models;

public partial class VUsersDetail
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public bool? IsBanned { get; set; }
}
