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

    public string? Gender { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? UserHasTag { get; set; }

    public string? ProfilePicture { get; set; }

    public string? Interests { get; set; }

    public string? PersonalDescription { get; set; }

    public string? Location { get; set; }

    public string? RelationshipStatus { get; set; }

    public string? LookingFor { get; set; }

    public string? PrivacySettings { get; set; }

    public string? SocialLinks { get; set; }

    public bool? IsBanned { get; set; }

    public string? Avatar { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
