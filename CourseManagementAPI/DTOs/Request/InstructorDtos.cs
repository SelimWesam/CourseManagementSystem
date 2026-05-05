using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs.Request;

public class CreateInstructorDto
{
    [Required]
    [MaxLength(100)]
    [MinLength(2)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}

public class UpdateInstructorDto
{
    [MaxLength(100)]
    [MinLength(2)]
    public string? FullName { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
}
