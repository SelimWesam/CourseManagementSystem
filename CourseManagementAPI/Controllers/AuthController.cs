using CourseManagementAPI.DTOs.Request;
using CourseManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagementAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Login as Admin, Instructor, or Student. Returns a JWT token.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(dto);
        if (result is null)
            return Unauthorized(new { message = "Invalid email, password, or role." });

        return Ok(result);
    }
}
