using System.ComponentModel.DataAnnotations;

namespace GestorOpiniones.Api.Models;

public class Post
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;
    [Required]
    public string Content { get; set; } = string.Empty;
    [Required]
    public string AuthorId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
