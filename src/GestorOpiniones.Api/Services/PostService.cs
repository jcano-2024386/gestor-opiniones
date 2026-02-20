using GestorOpiniones.Api.DTOs;
using GestorOpiniones.Api.Models;
using GestorOpiniones.Api.Repositories;

namespace GestorOpiniones.Api.Services;

public class PostService
{
    private readonly PostRepository _repo;
    public PostService(PostRepository repo) => _repo = repo;

    public async Task<PostDto> CreateAsync(CreatePostDto dto, string authorId)
    {
        var p = new Post { Title = dto.Title, Category = dto.Category, Content = dto.Content, AuthorId = authorId };
        var created = await _repo.CreateAsync(p);
        return Map(created);
    }

    public async Task<IEnumerable<PostDto>> ListAsync() => (await _repo.ListAsync()).Select(Map);
    public async Task<PostDto> GetByIdAsync(string id) => Map(await _repo.GetByIdAsync(id) ?? throw new InvalidOperationException("Post not found"));

    public async Task<PostDto> UpdateAsync(string id, UpdatePostDto dto, string authorId)
    {
        var post = await _repo.GetByIdAsync(id) ?? throw new InvalidOperationException("Post not found");
        if (post.AuthorId != authorId) throw new UnauthorizedAccessException("Only author can edit the post");
        post.Title = dto.Title; post.Category = dto.Category; post.Content = dto.Content;
        await _repo.UpdateAsync(post);
        return Map(post);
    }

    public async Task DeleteAsync(string id, string authorId)
    {
        var post = await _repo.GetByIdAsync(id) ?? throw new InvalidOperationException("Post not found");
        if (post.AuthorId != authorId) throw new UnauthorizedAccessException("Only author can delete the post");
        await _repo.DeleteAsync(id);
    }

    private static PostDto Map(Post p) => new PostDto { Id = p.Id, Title = p.Title, Category = p.Category, Content = p.Content, AuthorId = p.AuthorId, CreatedAt = p.CreatedAt };
}
