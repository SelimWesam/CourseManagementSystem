using CourseManagementAPI.DTOs.Request;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    /// <summary>Get all enrollments. Admin only.</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _enrollmentService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>Get a single enrollment by ID. Admin and Instructor.</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Instructor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _enrollmentService.GetByIdAsync(id);
        if (result is null) return NotFound(new { message = $"Enrollment with ID {id} not found." });
        return Ok(result);
    }

    /// <summary>Enroll a student in a course. Admin only.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateEnrollmentDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _enrollmentService.CreateAsync(dto);
        if (result is null)
            return Conflict(new { message = "Enrollment already exists or student/course not found." });

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Update grade for an enrollment. Admin and Instructor only.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Instructor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateGrade(int id, [FromBody] UpdateEnrollmentDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _enrollmentService.UpdateGradeAsync(id, dto);
        if (result is null) return NotFound(new { message = $"Enrollment with ID {id} not found." });
        return Ok(result);
    }

    /// <summary>Remove an enrollment. Admin only.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _enrollmentService.DeleteAsync(id);
        if (!deleted) return NotFound(new { message = $"Enrollment with ID {id} not found." });
        return NoContent();
    }
}
