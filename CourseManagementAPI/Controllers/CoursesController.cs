using CourseManagementAPI.DTOs.Request;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    /// <summary>Get all courses. All authenticated users can view courses.</summary>
    [HttpGet]
    [Authorize(Roles = "Admin,Instructor,Student")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _courseService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>Get a course by ID. All authenticated users.</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Instructor,Student")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _courseService.GetByIdAsync(id);
        if (result is null) return NotFound(new { message = $"Course with ID {id} not found." });
        return Ok(result);
    }

    /// <summary>Create a new course. Admin and Instructor only.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Instructor")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _courseService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Update a course. Admin and Instructor only.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin,Instructor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCourseDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _courseService.UpdateAsync(id, dto);
        if (result is null) return NotFound(new { message = $"Course with ID {id} not found." });
        return Ok(result);
    }

    /// <summary>Delete a course. Admin only.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _courseService.DeleteAsync(id);
        if (!deleted) return NotFound(new { message = $"Course with ID {id} not found." });
        return NoContent();
    }

    /// <summary>Get all students enrolled in a course. Admin and Instructor only.</summary>
    [HttpGet("{id:int}/students")]
    [Authorize(Roles = "Admin,Instructor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEnrolledStudents(int id)
    {
        var course = await _courseService.GetByIdAsync(id);
        if (course is null) return NotFound(new { message = $"Course with ID {id} not found." });

        var students = await _courseService.GetEnrolledStudentsAsync(id);
        return Ok(students);
    }
}
