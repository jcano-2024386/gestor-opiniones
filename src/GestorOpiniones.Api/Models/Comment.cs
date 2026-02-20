using System.ComponentModel.DataAnnotations;

namespace GestorOpiniones.Api.Models;


//Comentadooooooooo poooooooooooooooor Jeferson Andre Cano Lopez - 2024386
public class Comment
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public string PostId { get; set; } = string.Empty;
    [Required]
    public string AuthorId { get; set; } = string.Empty;
    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
