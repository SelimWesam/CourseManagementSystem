namespace CourseManagementAPI.Models;

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Student";
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;

    // Many-to-Many: Student enrolls in many Courses
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
