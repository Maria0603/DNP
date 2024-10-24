using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using ApiContracts;
using Entities;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController(IPostRepository postRepository, IUserRepository userRepository) : ControllerBase {

    [HttpPost]
    public async Task<ActionResult<PostDto>> AddPost([FromBody] CreatePostDto request) {
        Post post = new Post {
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId
        };
        Post createdPost = await postRepository.AddAsync(post);
        PostDto postDto = new() {
            Id = createdPost.Id,
            Title = createdPost.Title,
            Body = createdPost.Body,
            Author = userRepository.GetSingleAsync(createdPost.UserId).Result.Username
        };
        return Created($"/Posts/{postDto.Id}", postDto);
    }

    [HttpGet]
    public ActionResult<IEnumerable<PostDto>> GetPosts([FromQuery] string? title) {
        IEnumerable<Post> posts = postRepository.GetMany();

        if (title != null) {
            posts = posts.Where(post => post.Title.Contains(title));
        }
        IEnumerable<PostDto> postDtos = posts.Select(post => new PostDto {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            Author = userRepository.GetSingleAsync(post.UserId).Result.Username
        });
        return Ok(postDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PostDto>> GetPost(int id) {
        Post post = await postRepository.GetSingleAsync(id);
        PostDto postDto = new() {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            Author = userRepository.GetSingleAsync(post.UserId).Result.Username
        };
        return Ok(postDto);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePost(int id) {
        await postRepository.DeleteAsync(id);
        return NoContent();
    }
}