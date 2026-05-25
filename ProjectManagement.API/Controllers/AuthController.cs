using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Common;
using ProjectManagement.Application.Common.Interfaces;
using ProjectManagement.Application.DTOs.Auth;
using ProjectManagement.Application.Validators.Auth;

namespace ProjectManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        // Validate request
        var validator = new RegisterRequestDtoValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse.FailureResponse(
                validationResult.Errors.Select(e => e.ErrorMessage).FirstOrDefault() ?? "Validation failed"));
        }

        var result = await _authService.RegisterAsync(request);
        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, "User registered successfully"));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        // Validate request
        var validator = new LoginRequestDtoValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse.FailureResponse(
                validationResult.Errors.Select(e => e.ErrorMessage).FirstOrDefault() ?? "Validation failed"));
        }

        var result = await _authService.LoginAsync(request);
        return Ok(ApiResponse<AuthResponseDto>.SuccessResponse(result, "Login successful"));
    }
}