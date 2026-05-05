using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs.Request;
using CourseManagementAPI.DTOs.Response;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public class StudentService : IStudentService
{
    private readonly AppDbContext _db;

    public StudentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<StudentResponseDto>> GetAllAsync()
    {
        return await _db.Students
            .AsNoTracking()
            .Select(s => new StudentResponseDto
            {
                Id = s.Id,
                FullName = s.FullName,
                Email = s.Email,
                Role = s.Role,
                EnrolledAt = s.EnrolledAt
            })
            .ToListAsync();
    }

    public async Task<StudentResponseDto?> GetByIdAsync(int id)
    {
        return await _db.Students
            .AsNoTracking()
            .Where(s => s.Id == id)
            .Select(s => new StudentResponseDto
            {
                Id = s.Id,
                FullName = s.FullName,
                Email = s.Email,
                Role = s.Role,
                EnrolledAt = s.EnrolledAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<StudentResponseDto> CreateAsync(CreateStudentDto dto)
    {
        var student = new Student
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "Student"
        };

        _db.Students.Add(student);
        await _db.SaveChangesAsync();

        return new StudentResponseDto
        {
            Id = student.Id,
            FullName = student.FullName,
            Email = student.Email,
            Role = student.Role,
            EnrolledAt = student.EnrolledAt
        };
    }

    public async Task<StudentResponseDto?> UpdateAsync(int id, UpdateStudentDto dto)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null) return null;

        if (dto.FullName is not null) student.FullName = dto.FullName;
        if (dto.Email is not null) student.Email = dto.Email;

        await _db.SaveChangesAsync();

        return new StudentResponseDto
        {
            Id = student.Id,
            FullName = student.FullName,
            Email = student.Email,
            Role = student.Role,
            EnrolledAt = student.EnrolledAt
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null) return false;

        _db.Students.Remove(student);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<CourseResponseDto>> GetEnrolledCoursesAsync(int studentId)
    {
        return await _db.Enrollments
            .AsNoTracking()
            .Where(e => e.StudentId == studentId)
            .Select(e => new CourseResponseDto
            {
                Id = e.Course.Id,
                Title = e.Course.Title,
                Description = e.Course.Description,
                Credits = e.Course.Credits,
                CreatedAt = e.Course.CreatedAt,
                InstructorName = e.Course.Instructor.FullName,
                EnrollmentCount = e.Course.Enrollments.Count
            })
            .ToListAsync();
    }
}
