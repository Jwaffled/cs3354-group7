using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TextbookExchangeApp.Models;
using TextbookExchangeApp.Services.Auth;
using TextbookExchangeApp.Services.Auth.Dto;

namespace TextbookExchangeApp.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthService _authService;

    public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IAuthService authService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _authService = authService;
    }

    [HttpPost("create-account")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
    {
        var result = await _authService.CreateAccountAsync(dto);
        if (result.Succeeded)
        {
            return Ok(new { message = "Account Created Successfully" });
        }
        
        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AccountLoginDto dto)
    {
        var result = await _authService.LoginUserAsync(dto);
        if (result)
        {
            return Ok(new { message = "Logged in successfully." });
        }
        
        return Unauthorized(new { message = "Invalid username or password." });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { message = "Signed out successfully." });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound(new { message = "User not found." });
        }

        return Ok(new AccountInfoDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
        });
    }
}