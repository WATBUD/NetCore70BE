using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCore60.Models;

public partial class VUsersDetailDTO
{
    [Required]
    public int? UserId { get; set; } = null!;

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? Gender { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? UserHasTag { get; set; }

    public IFormFile? ProfilePicture { get; set; }

    public string? Interests { get; set; }

    public string? PersonalDescription { get; set; }

    public string? Location { get; set; }

    public string? RelationshipStatus { get; set; }

    public string? LookingFor { get; set; }

    public string? PrivacySettings { get; set; }

    public string? SocialLinks { get; set; }
}
