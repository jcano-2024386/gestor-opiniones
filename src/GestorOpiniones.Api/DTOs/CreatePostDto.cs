using System.ComponentModel.DataAnnotations;

namespace GestorOpiniones.Api.DTOs;

public class CreatePostDto
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;
    [Required]
    public string Content { get; set; } = string.Empty;
}
