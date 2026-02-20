using GestorOpiniones.Api.DTOs;
using GestorOpiniones.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestorOpiniones.Api.Controllers;


//Jeferson Andre Cano Lopez - 2024386

[ApiController]
[Route("api/v1/[controller]")]
public class PostsController : ControllerBase
{
    private readonly PostService _postService;
    public PostsController(PostService postService) => _postService = postService;

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<PostDto>> Create([FromBody] CreatePostDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
        var created = await _postService.CreateAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostDto>>> List() => Ok(await _postService.ListAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetById(string id) => Ok(await _postService.GetByIdAsync(id));

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<PostDto>> Update(string id, [FromBody] UpdatePostDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
        var updated = await _postService.UpdateAsync(id, dto, userId);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
        await _postService.DeleteAsync(id, userId);
        return NoContent();
    }
}
