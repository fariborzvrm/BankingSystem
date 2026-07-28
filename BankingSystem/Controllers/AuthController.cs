using BankingSystem.Application.DTOs;
using BankingSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.API.Controllers;

    [ApiController]
    [Route("api/authentication")]


    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost]

    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var response = await _authService.RegisterAsync(registerRequestDto);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

    [HttpPost]

    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var response = await _authService.LoginAsync(loginRequestDto);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }
        return Ok(response);
    }

}

