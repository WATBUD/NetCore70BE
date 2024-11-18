using System;
using System.Collections.Generic;

namespace NetCore60.Models;

public partial class VUserRolePermission
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public int PermissionId { get; set; }

    public string PermissionName { get; set; } = null!;
}
