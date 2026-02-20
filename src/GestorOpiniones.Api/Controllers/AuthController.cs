using GestorOpiniones.Api.DTOs;
using GestorOpiniones.Api.Services;
using Microsoft.AspNetCore.Mvc;


//Jeferson Andre Cano Lopez - 2024386


namespace GestorOpiniones.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;
    public AuthController(AuthService auth) => _auth = auth;

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto dto)
    {
        var res = await _auth.RegisterAsync(dto);
        return Created(string.Empty, res);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var res = await _auth.LoginAsync(dto);
        return Ok(res);
    }
}
