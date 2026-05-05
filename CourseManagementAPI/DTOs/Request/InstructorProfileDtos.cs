using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs.Request;

public class CreateInstructorProfileDto
{
    [Required]
    [MaxLength(500)]
    public string Bio { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Department { get; set; } = string.Empty;

    [MaxLength(100)]
    public string OfficeLocation { get; set; } = string.Empty;

    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;
}

public class UpdateInstructorProfileDto
{
    [MaxLength(500)]
    public string? Bio { get; set; }

    [MaxLength(100)]
    public string? Department { get; set; }

    [MaxLength(100)]
    public string? OfficeLocation { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }
}
