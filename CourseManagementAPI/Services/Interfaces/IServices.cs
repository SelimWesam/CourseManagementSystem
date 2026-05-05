using CourseManagementAPI.DTOs.Request;
using CourseManagementAPI.DTOs.Response;

namespace CourseManagementAPI.Services.Interfaces;

public interface IInstructorService
{
    Task<IEnumerable<InstructorResponseDto>> GetAllAsync();
    Task<InstructorResponseDto?> GetByIdAsync(int id);
    Task<InstructorResponseDto> CreateAsync(CreateInstructorDto dto);
    Task<InstructorResponseDto?> UpdateAsync(int id, UpdateInstructorDto dto);
    Task<bool> DeleteAsync(int id);
    Task<InstructorResponseDto?> AddProfileAsync(int instructorId, CreateInstructorProfileDto dto);
    Task<InstructorResponseDto?> UpdateProfileAsync(int instructorId, UpdateInstructorProfileDto dto);
}

public interface IStudentService
{
    Task<IEnumerable<StudentResponseDto>> GetAllAsync();
    Task<StudentResponseDto?> GetByIdAsync(int id);
    Task<StudentResponseDto> CreateAsync(CreateStudentDto dto);
    Task<StudentResponseDto?> UpdateAsync(int id, UpdateStudentDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<CourseResponseDto>> GetEnrolledCoursesAsync(int studentId);
}

public interface ICourseService
{
    Task<IEnumerable<CourseResponseDto>> GetAllAsync();
    Task<CourseResponseDto?> GetByIdAsync(int id);
    Task<CourseResponseDto> CreateAsync(CreateCourseDto dto);
    Task<CourseResponseDto?> UpdateAsync(int id, UpdateCourseDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<StudentResponseDto>> GetEnrolledStudentsAsync(int courseId);
}

public interface IEnrollmentService
{
    Task<IEnumerable<EnrollmentResponseDto>> GetAllAsync();
    Task<EnrollmentResponseDto?> GetByIdAsync(int id);
    Task<EnrollmentResponseDto?> CreateAsync(CreateEnrollmentDto dto);
    Task<EnrollmentResponseDto?> UpdateGradeAsync(int id, UpdateEnrollmentDto dto);
    Task<bool> DeleteAsync(int id);
}

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
}
