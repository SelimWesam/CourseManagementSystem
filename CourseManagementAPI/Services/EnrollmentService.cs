using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs.Request;
using CourseManagementAPI.DTOs.Response;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public class EnrollmentService : IEnrollmentService
{
    private readonly AppDbContext _db;

    public EnrollmentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<EnrollmentResponseDto>> GetAllAsync()
    {
        return await _db.Enrollments
            .AsNoTracking()
            .Select(e => new EnrollmentResponseDto
            {
                Id = e.Id,
                StudentName = e.Student.FullName,
                CourseName = e.Course.Title,
                Grade = e.Grade,
                EnrolledAt = e.EnrolledAt
            })
            .ToListAsync();
    }

    public async Task<EnrollmentResponseDto?> GetByIdAsync(int id)
    {
        return await _db.Enrollments
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new EnrollmentResponseDto
            {
                Id = e.Id,
                StudentName = e.Student.FullName,
                CourseName = e.Course.Title,
                Grade = e.Grade,
                EnrolledAt = e.EnrolledAt
            })
            .FirstOrDefaultAsync();
    }

    public async Task<EnrollmentResponseDto?> CreateAsync(CreateEnrollmentDto dto)
    {
        // Validate student and course exist
        var studentExists = await _db.Students.AnyAsync(s => s.Id == dto.StudentId);
        var courseExists = await _db.Courses.AnyAsync(c => c.Id == dto.CourseId);
        if (!studentExists || !courseExists) return null;

        // Prevent duplicate enrollment
        var alreadyEnrolled = await _db.Enrollments
            .AnyAsync(e => e.StudentId == dto.StudentId && e.CourseId == dto.CourseId);
        if (alreadyEnrolled) return null;

        var enrollment = new Enrollment
        {
            StudentId = dto.StudentId,
            CourseId = dto.CourseId
        };

        _db.Enrollments.Add(enrollment);
        await _db.SaveChangesAsync();

        return await GetByIdAsync(enrollment.Id);
    }

    public async Task<EnrollmentResponseDto?> UpdateGradeAsync(int id, UpdateEnrollmentDto dto)
    {
        var enrollment = await _db.Enrollments.FindAsync(id);
        if (enrollment is null) return null;

        if (dto.Grade is not null) enrollment.Grade = dto.Grade;

        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var enrollment = await _db.Enrollments.FindAsync(id);
        if (enrollment is null) return false;

        _db.Enrollments.Remove(enrollment);
        await _db.SaveChangesAsync();
        return true;
    }
}
