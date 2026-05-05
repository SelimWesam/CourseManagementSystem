using CourseManagementAPI.DTOs.Request;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    /// <summary>Get all students. Admin only.</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _studentService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>Get a student by ID. Admin and the student themselves.</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Student")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _studentService.GetByIdAsync(id);
        if (result is null) return NotFound(new { message = $"Student with ID {id} not found." });
        return Ok(result);
    }

    /// <summary>Register a new student. Public endpoint.</summary>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _studentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Update a student. Admin only.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateStudentDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _studentService.UpdateAsync(id, dto);
        if (result is null) return NotFound(new { message = $"Student with ID {id} not found." });
        return Ok(result);
    }

    /// <summary>Delete a student. Admin only.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _studentService.DeleteAsync(id);
        if (!deleted) return NotFound(new { message = $"Student with ID {id} not found." });
        return NoContent();
    }

    /// <summary>Get all courses a student is enrolled in. Admin and Instructor.</summary>
    [HttpGet("{id:int}/courses")]
    [Authorize(Roles = "Admin,Instructor,Student")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEnrolledCourses(int id)
    {
        var student = await _studentService.GetByIdAsync(id);
        if (student is null) return NotFound(new { message = $"Student with ID {id} not found." });

        var courses = await _studentService.GetEnrolledCoursesAsync(id);
        return Ok(courses);
    }
}
