using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs.Request;
using CourseManagementAPI.DTOs.Response;
using CourseManagementAPI.Helpers;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _db;
    private readonly JwtHelper _jwt;

    public AuthService(AppDbContext db, JwtHelper jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var role = dto.Role.Trim();

        return role switch
        {
            "Admin" => await LoginAdmin(dto),
            "Instructor" => await LoginInstructor(dto),
            "Student" => await LoginStudent(dto),
            _ => null
        };
    }

    private async Task<AuthResponseDto?> LoginAdmin(LoginDto dto)
    {
        var admin = await _db.Admins
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Email == dto.Email);

        if (admin is null || !BCrypt.Net.BCrypt.Verify(dto.Password, admin.PasswordHash))
            return null;

        return new AuthResponseDto
        {
            Token = _jwt.GenerateToken(admin.Id, admin.Email, admin.FullName, admin.Role),
            Role = admin.Role,
            Email = admin.Email,
            FullName = admin.FullName
        };
    }

    private async Task<AuthResponseDto?> LoginInstructor(LoginDto dto)
    {
        var instructor = await _db.Instructors
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Email == dto.Email);

        if (instructor is null || !BCrypt.Net.BCrypt.Verify(dto.Password, instructor.PasswordHash))
            return null;

        return new AuthResponseDto
        {
            Token = _jwt.GenerateToken(instructor.Id, instructor.Email, instructor.FullName, instructor.Role),
            Role = instructor.Role,
            Email = instructor.Email,
            FullName = instructor.FullName
        };
    }

    private async Task<AuthResponseDto?> LoginStudent(LoginDto dto)
    {
        var student = await _db.Students
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Email == dto.Email);

        if (student is null || !BCrypt.Net.BCrypt.Verify(dto.Password, student.PasswordHash))
            return null;

        return new AuthResponseDto
        {
            Token = _jwt.GenerateToken(student.Id, student.Email, student.FullName, student.Role),
            Role = student.Role,
            Email = student.Email,
            FullName = student.FullName
        };
    }
}
