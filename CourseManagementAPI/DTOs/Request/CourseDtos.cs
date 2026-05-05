using System.ComponentModel.DataAnnotations;

namespace CourseManagementAPI.DTOs.Request;

public class CreateCourseDto
{
    [Required]
    [MaxLength(150)]
    [MinLength(3)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(1, 6)]
    public int Credits { get; set; }

    [Required]
    public int InstructorId { get; set; }
}

public class UpdateCourseDto
{
    [MaxLength(150)]
    [MinLength(3)]
    public string? Title { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Range(1, 6)]
    public int? Credits { get; set; }
}
