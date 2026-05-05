using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs.Request;
using CourseManagementAPI.DTOs.Response;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public class CourseService : ICourseService
{
    private readonly AppDbContext _db;

    public CourseService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CourseResponseDto>> GetAllAsync()
    {
        return await _db.Courses
            .AsNoTracking()
            .Select(c => new CourseResponseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Credits = c.Credits,
                CreatedAt = c.CreatedAt,
                InstructorName = c.Instructor.FullName,
                EnrollmentCount = c.Enrollments.Count
            })
            .ToListAsync();
    }

    public async Task<CourseResponseDto?> GetByIdAsync(int id)
    {
        return await _db.Courses
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new CourseResponseDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Credits = c.Credits,
                CreatedAt = c.CreatedAt,
                InstructorName = c.Instructor.FullName,
                EnrollmentCount = c.Enrollments.Count
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CourseResponseDto> CreateAsync(CreateCourseDto dto)
    {
        var course = new Course
        {
            Title = dto.Title,
            Description = dto.Description,
            Credits = dto.Credits,
            InstructorId = dto.InstructorId
        };

        _db.Courses.Add(course);
        await _db.SaveChangesAsync();

        // Re-fetch with instructor name
        return (await GetByIdAsync(course.Id))!;
    }

    public async Task<CourseResponseDto?> UpdateAsync(int id, UpdateCourseDto dto)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course is null) return null;

        if (dto.Title is not null) course.Title = dto.Title;
        if (dto.Description is not null) course.Description = dto.Description;
        if (dto.Credits.HasValue) course.Credits = dto.Credits.Value;

        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _db.Courses.FindAsync(id);
        if (course is null) return false;

        _db.Courses.Remove(course);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<StudentResponseDto>> GetEnrolledStudentsAsync(int courseId)
    {
        return await _db.Enrollments
            .AsNoTracking()
            .Where(e => e.CourseId == courseId)
            .Select(e => new StudentResponseDto
            {
                Id = e.Student.Id,
                FullName = e.Student.FullName,
                Email = e.Student.Email,
                Role = e.Student.Role,
                EnrolledAt = e.Student.EnrolledAt
            })
            .ToListAsync();
    }
}
