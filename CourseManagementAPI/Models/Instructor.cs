namespace CourseManagementAPI.Models;

public class Instructor
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Instructor";

    // One-to-One: Instructor has one InstructorProfile
    public InstructorProfile? Profile { get; set; }

    // One-to-Many: Instructor teaches many Courses
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
