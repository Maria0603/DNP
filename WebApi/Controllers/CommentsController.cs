using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using ApiContracts;
using Entities;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentsController(
    ICommentRepository commentRepository,
    IUserRepository userRepository) : ControllerBase {
    //private readonly ICommentRepository commentRepository;

    [HttpPost]
    public async Task<ActionResult<CommentDto>> AddComment(
        [FromBody] CreateCommentDto request) {
        Comment comment = new Comment {
            Body = request.Body,
            PostId = request.PostId,
            UserId = request.UserId
        };
        Comment createdComment = await commentRepository.AddAsync(comment);
        CommentDto commentDto = new() {
            Id = createdComment.Id,
            Body = createdComment.Body,
            PostId = createdComment.PostId,
            AuthorUsername = userRepository
                .GetSingleAsync(createdComment.UserId).Result.Username
        };
        return Created($"/Comments/{commentDto.Id}", commentDto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommentDto>> GetComments(
        [FromQuery] int? postId) {
        IEnumerable<Comment> comments = commentRepository.GetMany();

        if (postId.HasValue) {
            comments =
                comments.Where(comment => comment.PostId == postId.Value);
        }

        IEnumerable<CommentDto> commentDtos = comments.Select(comment =>
            new CommentDto {
                Id = comment.Id,
                Body = comment.Body,
                PostId = comment.PostId,
                AuthorUsername = userRepository.GetSingleAsync(comment.UserId)
                    .Result.Username
            });
        return Ok(commentDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CommentDto>> GetComment(int id) {
        Comment comment = await commentRepository.GetSingleAsync(id);
        CommentDto commentDto = new() {
            Id = comment.Id,
            Body = comment.Body,
            PostId = comment.PostId,
            AuthorUsername = userRepository.GetSingleAsync(comment.UserId)
                .Result.Username
        };
        return Ok(commentDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteComment(int id) {
        await commentRepository.DeleteAsync(id);
        return NoContent();
    }
}