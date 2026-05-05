namespace CourseManagementAPI.Models;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Credits { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // One-to-Many FK: Course belongs to one Instructor
    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; } = null!;

    // Many-to-Many: Course has many Students via Enrollment
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
}
