using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TextbookExchangeApp.Models;
using TextbookExchangeApp.Services.Login;
using TextbookExchangeApp.Services.Login.Dto;

namespace TextbookExchangeApp.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILoginService _loginService;

    public AuthController(SignInManager<ApplicationUser> signInManager, ILoginService loginService)
    {
        _signInManager = signInManager;
        _loginService = loginService;
    }

    [HttpPost("create-account")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
    {
        var result = await _loginService.CreateAccountAsync(dto);
        if (result.Succeeded)
        {
            return Ok(new { message = "Account Created Successfully" });
        }
        
        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AccountLoginDto dto)
    {
        var result = await _loginService.LoginUserAsync(dto);
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
}