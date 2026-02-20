using GestorOpiniones.Api.DTOs;
using GestorOpiniones.Api.Models;
using GestorOpiniones.Api.Repositories;

namespace GestorOpiniones.Api.Services;

public class CommentService
{
    private readonly CommentRepository _repo;
    public CommentService(CommentRepository repo) => _repo = repo;

    public async Task<CommentDto> CreateAsync(CreateCommentDto dto, string authorId)
    {
        var c = new Comment { PostId = dto.PostId, Content = dto.Content, AuthorId = authorId };
        var created = await _repo.CreateAsync(c);
        return Map(created);
    }

    public async Task<IEnumerable<CommentDto>> ListByPostAsync(string postId) => (await _repo.ListByPostAsync(postId)).Select(Map);

    public async Task<CommentDto> UpdateAsync(string id, string content, string authorId)
    {
        var comment = await _repo.GetByIdAsync(id) ?? throw new InvalidOperationException("Comment not found");
        if (comment.AuthorId != authorId) throw new UnauthorizedAccessException("Only author can edit comment");
        comment.Content = content; await _repo.UpdateAsync(comment);
        return Map(comment);
    }

    public async Task DeleteAsync(string id, string authorId)
    {
        var comment = await _repo.GetByIdAsync(id) ?? throw new InvalidOperationException("Comment not found");
        if (comment.AuthorId != authorId) throw new UnauthorizedAccessException("Only author can delete comment");
        await _repo.DeleteAsync(id);
    }

    private static CommentDto Map(Comment c) => new CommentDto { Id = c.Id, PostId = c.PostId, AuthorId = c.AuthorId, Content = c.Content, CreatedAt = c.CreatedAt };
}
