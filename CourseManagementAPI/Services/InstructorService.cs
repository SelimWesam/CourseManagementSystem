using CourseManagementAPI.Data;
using CourseManagementAPI.DTOs.Request;
using CourseManagementAPI.DTOs.Response;
using CourseManagementAPI.Models;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementAPI.Services;

public class InstructorService : IInstructorService
{
    private readonly AppDbContext _db;

    public InstructorService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<InstructorResponseDto>> GetAllAsync()
    {
        return await _db.Instructors
            .AsNoTracking()
            .Include(i => i.Profile)
            .Select(i => MapToDto(i))
            .ToListAsync();
    }

    public async Task<InstructorResponseDto?> GetByIdAsync(int id)
    {
        return await _db.Instructors
            .AsNoTracking()
            .Include(i => i.Profile)
            .Where(i => i.Id == id)
            .Select(i => MapToDto(i))
            .FirstOrDefaultAsync();
    }

    public async Task<InstructorResponseDto> CreateAsync(CreateInstructorDto dto)
    {
        var instructor = new Instructor
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "Instructor"
        };

        _db.Instructors.Add(instructor);
        await _db.SaveChangesAsync();
        return MapToDto(instructor);
    }

    public async Task<InstructorResponseDto?> UpdateAsync(int id, UpdateInstructorDto dto)
    {
        var instructor = await _db.Instructors.Include(i => i.Profile).FirstOrDefaultAsync(i => i.Id == id);
        if (instructor is null) return null;

        if (dto.FullName is not null) instructor.FullName = dto.FullName;
        if (dto.Email is not null) instructor.Email = dto.Email;

        await _db.SaveChangesAsync();
        return MapToDto(instructor);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var instructor = await _db.Instructors.FindAsync(id);
        if (instructor is null) return false;

        _db.Instructors.Remove(instructor);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<InstructorResponseDto?> AddProfileAsync(int instructorId, CreateInstructorProfileDto dto)
    {
        var instructor = await _db.Instructors.Include(i => i.Profile).FirstOrDefaultAsync(i => i.Id == instructorId);
        if (instructor is null) return null;
        if (instructor.Profile is not null) return null; // already exists

        instructor.Profile = new InstructorProfile
        {
            Bio = dto.Bio,
            Department = dto.Department,
            OfficeLocation = dto.OfficeLocation,
            PhoneNumber = dto.PhoneNumber
        };

        await _db.SaveChangesAsync();
        return MapToDto(instructor);
    }

    public async Task<InstructorResponseDto?> UpdateProfileAsync(int instructorId, UpdateInstructorProfileDto dto)
    {
        var instructor = await _db.Instructors.Include(i => i.Profile).FirstOrDefaultAsync(i => i.Id == instructorId);
        if (instructor is null || instructor.Profile is null) return null;

        if (dto.Bio is not null) instructor.Profile.Bio = dto.Bio;
        if (dto.Department is not null) instructor.Profile.Department = dto.Department;
        if (dto.OfficeLocation is not null) instructor.Profile.OfficeLocation = dto.OfficeLocation;
        if (dto.PhoneNumber is not null) instructor.Profile.PhoneNumber = dto.PhoneNumber;

        await _db.SaveChangesAsync();
        return MapToDto(instructor);
    }

    private static InstructorResponseDto MapToDto(Instructor i) => new()
    {
        Id = i.Id,
        FullName = i.FullName,
        Email = i.Email,
        Role = i.Role,
        Profile = i.Profile is null ? null : new InstructorProfileResponseDto
        {
            Id = i.Profile.Id,
            Bio = i.Profile.Bio,
            Department = i.Profile.Department,
            OfficeLocation = i.Profile.OfficeLocation,
            PhoneNumber = i.Profile.PhoneNumber
        }
    };
}
