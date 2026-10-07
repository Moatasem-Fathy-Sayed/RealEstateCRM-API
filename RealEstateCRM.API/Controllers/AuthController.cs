using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RealEstateCRM.Core.DTOs;
using RealEstateCRM.Core.Entities;
using RealEstateCRM.Core.Interfaces;

namespace RealEstateCRM.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthService _authService;

    public AuthController(UserManager<ApplicationUser> userManager, IAuthService authService)
    {
        _userManager = userManager;
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var userExists = await _userManager.FindByEmailAsync(dto.Email);
        if (userExists != null)
            return BadRequest(new { Message = "Email already exists." });

        var user = new ApplicationUser
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            UserName = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { Message = "User registered successfully." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return Unauthorized(new { Message = "Invalid email or password." });

        var token = await _authService.GenerateJwtTokenAsync(user);

        return Ok(new AuthResponseDto
        {
            Token = token,
            Email = user.Email!,
            ExpiresAt = DateTime.UtcNow.AddHours(3)
        });
    }
}