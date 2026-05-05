namespace CourseManagementAPI.DTOs.Response;

public class InstructorResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public InstructorProfileResponseDto? Profile { get; set; }
}

public class InstructorProfileResponseDto
{
    public int Id { get; set; }
    public string Bio { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string OfficeLocation { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public class StudentResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime EnrolledAt { get; set; }
}

public class CourseResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Credits { get; set; }
    public DateTime CreatedAt { get; set; }
    public string InstructorName { get; set; } = string.Empty;
    public int EnrollmentCount { get; set; }
}

public class EnrollmentResponseDto
{
    public int Id { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string CourseName { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;
    public DateTime EnrolledAt { get; set; }
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}
