using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs.Request;

public class CreateEnrollmentDto
{
    [Required]
    public int StudentId { get; set; }

    [Required]
    public int CourseId { get; set; }
}

public class UpdateEnrollmentDto
{
    [MaxLength(2)]
    public string? Grade { get; set; }
}

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    /// <summary>Accepted values: Admin, Instructor, Student</summary>
    [Required]
    public string Role { get; set; } = string.Empty;
}
