using BirthdayNotifier.Core.DTOs;
using BirthdayNotifier.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BirthdayNotifier.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace BirthdayNotifier.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            Topic = $"birthdays-{Guid.NewGuid()}"
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        var token = _jwtTokenService.GenerateToken(user);
        return Ok(new { Token = token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Unauthorized("Invalid credentials");

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded)
            return Unauthorized("Invalid credentials");

        var token = _jwtTokenService.GenerateToken(user);
        return Ok(new { Token = token });
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("secure-test")]
    public IActionResult SecureTest()
    {
        if (!User.Identity.IsAuthenticated)
            return Unauthorized("No valid token");

        return Ok("You are authorized");
    }
}