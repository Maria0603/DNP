using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using ApiContracts;
using Entities;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController(
    IPostRepository postRepository,
    IUserRepository userRepository,
    ICommentRepository commentRepository) : ControllerBase {
    [HttpPost]
    public async Task<ActionResult<GetPostDto>> AddPost(
        [FromBody] CreatePostDto request) {
        Post post = new Post {
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId
        };
        Post createdPost = await postRepository.AddAsync(post);
        GetPostDto getPostDto = new() {
            PostId = createdPost.Id,
            Title = createdPost.Title,
            Body = createdPost.Body,
            AuthorName = userRepository.GetSingleAsync(createdPost.UserId)
                .Result.Username
        };
        return Created($"/Posts/{getPostDto.PostId}", getPostDto);
    }
    

    [HttpGet]
    public async Task<ActionResult<List<GetPostDto>>> GetPostsAsync(
        [FromQuery] string? author) {
        List<GetPostDto> sendDto = new List<GetPostDto>();

        //extracts all posts
        foreach (Post post in postRepository.GetMany()) {
            var postDto = await GetPostByIdAsync(post.Id);
            sendDto.Add(postDto);
        }

        //filter them
        if (!string.IsNullOrEmpty(author))
            sendDto = sendDto.Where(p =>
                p.AuthorName.ToLower().Contains(author.ToLower())).ToList();

        if (sendDto.Count == 0)
            return NotFound("No posts found.");

        return Ok(sendDto);
    }
    
    
    [HttpGet("{id}")]
    public async Task<GetPostDto> GetPostByIdAsync(int id) {
        Post post = await postRepository.GetSingleAsync(id);

        //extract post's author username
        String authorUsername =
            (await userRepository.GetSingleAsync(post.UserId)).Username;

        //extract the comments
        List<GetCommentDto> comments = new();

        foreach (Comment comment in commentRepository.GetCommentsForPost(
                     post.Id))
            comments.Add(new GetCommentDto {
                Id = comment.Id,
                Body = comment.Body,
                PostId = id,
                AuthorId = comment.UserId,
                AuthorUsername =
                    (await userRepository.GetSingleAsync(comment.UserId))
                    .Username
            });

        GetPostDto sendDto = new() {
            Title = post.Title,
            Body = post.Body,
            PostId = post.Id,
            AuthorName = authorUsername,
            Comments = comments
        };

        return sendDto;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost(int id) {
        await postRepository.DeleteAsync(id);
        return NoContent();
    }
}