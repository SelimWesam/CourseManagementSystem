using CourseManagementAPI.DTOs.Request;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InstructorsController : ControllerBase
{
    private readonly IInstructorService _instructorService;

    public InstructorsController(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    /// <summary>Get all instructors. Accessible by Admin only.</summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _instructorService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>Get a single instructor by ID. Accessible by Admin and Instructor.</summary>
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Instructor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _instructorService.GetByIdAsync(id);
        if (result is null) return NotFound(new { message = $"Instructor with ID {id} not found." });
        return Ok(result);
    }

    /// <summary>Create a new instructor. Admin only.</summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateInstructorDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _instructorService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>Update an existing instructor. Admin only.</summary>
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateInstructorDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _instructorService.UpdateAsync(id, dto);
        if (result is null) return NotFound(new { message = $"Instructor with ID {id} not found." });
        return Ok(result);
    }

    /// <summary>Delete an instructor. Admin only.</summary>
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _instructorService.DeleteAsync(id);
        if (!deleted) return NotFound(new { message = $"Instructor with ID {id} not found." });
        return NoContent();
    }

    /// <summary>Add a profile to an instructor. Admin or the Instructor themselves.</summary>
    [HttpPost("{id:int}/profile")]
    [Authorize(Roles = "Admin,Instructor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddProfile(int id, [FromBody] CreateInstructorProfileDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var instructor = await _instructorService.GetByIdAsync(id);
        if (instructor is null) return NotFound(new { message = $"Instructor with ID {id} not found." });
        if (instructor.Profile is not null) return Conflict(new { message = "Profile already exists for this instructor." });

        var result = await _instructorService.AddProfileAsync(id, dto);
        return Ok(result);
    }

    /// <summary>Update an instructor's profile. Admin or the Instructor themselves.</summary>
    [HttpPut("{id:int}/profile")]
    [Authorize(Roles = "Admin,Instructor")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateInstructorProfileDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _instructorService.UpdateProfileAsync(id, dto);
        if (result is null) return NotFound(new { message = $"Instructor or profile with ID {id} not found." });
        return Ok(result);
    }
}
