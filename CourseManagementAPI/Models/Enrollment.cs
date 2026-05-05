namespace CourseManagementAPI.Models;

public class Enrollment
{
    public int Id { get; set; }
    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public string Grade { get; set; } = "N/A";

    // Many-to-Many FKs
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}
