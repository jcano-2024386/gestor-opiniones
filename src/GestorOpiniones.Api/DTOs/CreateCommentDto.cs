using System.ComponentModel.DataAnnotations;

namespace GestorOpiniones.Api.DTOs;

public class CreateCommentDto
{
    [Required]
    public string PostId { get; set; } = string.Empty;
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;
}
