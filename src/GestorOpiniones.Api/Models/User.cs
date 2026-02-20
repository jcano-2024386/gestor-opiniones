using System.ComponentModel.DataAnnotations;

namespace GestorOpiniones.Api.Models;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
