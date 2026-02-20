using GestorOpiniones.Api.DTOs;
using GestorOpiniones.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;



//Jeferson Andre Cano Lopez - 2024386

namespace GestorOpiniones.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly CommentService _commentService;
    public CommentsController(CommentService commentService) => _commentService = commentService;

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CommentDto>> Create([FromBody] CreateCommentDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
        var created = await _commentService.CreateAsync(dto, userId);
        return Created(string.Empty, created);
    }

    [HttpGet("bypost/{postId}")]
    public async Task<ActionResult<IEnumerable<CommentDto>>> ListByPost(string postId) => Ok(await _commentService.ListByPostAsync(postId));

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<CommentDto>> Update(string id, [FromBody] string content)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
        var updated = await _commentService.UpdateAsync(id, content, userId);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(string id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (string.IsNullOrEmpty(userId)) return Unauthorized();
        await _commentService.DeleteAsync(id, userId);
        return NoContent();
    }
}
