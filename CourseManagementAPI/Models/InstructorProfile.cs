namespace CourseManagementAPI.Models;

public class InstructorProfile
{
    public int Id { get; set; }
    public string Bio { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string OfficeLocation { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    // One-to-One FK
    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; } = null!;
}
