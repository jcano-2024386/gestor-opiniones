using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestorOpiniones.Api.DTOs;
using GestorOpiniones.Api.Models;
using GestorOpiniones.Api.Repositories;
using GestorOpiniones.Api.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;

namespace GestorOpiniones.Api.Services;

public class AuthService
{
    private readonly UserRepository _users;
    private readonly JwtSettings _jwt;
    public AuthService(UserRepository users, IOptions<JwtSettings> jwt) { _users = users; _jwt = jwt.Value; }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var exists = await _users.GetByEmailAsync(dto.Email) ?? await _users.GetByUsernameAsync(dto.Username);
        if (exists != null) throw new InvalidOperationException("User already exists");
        var user = new User { Email = dto.Email, Username = dto.Username, PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password) };
        await _users.CreateAsync(user);
        var token = GenerateToken(user);
        return new AuthResponseDto { Token = token, UserId = user.Id };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        User? user = null;
        if (dto.EmailOrUsername.Contains("@")) user = await _users.GetByEmailAsync(dto.EmailOrUsername);
        else user = await _users.GetByUsernameAsync(dto.EmailOrUsername);
        if (user == null) throw new InvalidOperationException("Invalid credentials");
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) throw new InvalidOperationException("Invalid credentials");
        var token = GenerateToken(user);
        return new AuthResponseDto { Token = token, UserId = user.Id };
    }

    private string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, user.Id), new Claim(ClaimTypes.Name, user.Username) };
        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.ExpiryInMinutes > 0 ? _jwt.ExpiryInMinutes : 60),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
