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

    //TODO: Fix this to correctly return the author of the post. It also has to return the comments of the post
    //TODO: Input query parameter should be an id instead of a title.
    /*[HttpGet]
    public async Task<ActionResult<IEnumerable<GetPostDto>>> GetPosts(
        [FromQuery] string? title) {
        IEnumerable<Post> posts = postRepository.GetMany();

        if (title != null) {
            posts = posts.Where(post =>
                post.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        IEnumerable<GetPostDto> postDtos = await Task.WhenAll(posts.Select(
            async post => {
                var authorId = post.UserId;
                return new GetPostDto {
                    PostId = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    AuthorName = userRepository.GetSingleAsync(authorId).Result.Username
                };
            }));
        return Ok(postDtos);
    }*/

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


    /*public async Task<ActionResult<GetPostDto>> GetPost(int id) {
        Post post = await postRepository.GetSingleAsync(id);
        GetPostDto getPostDto = new() {
            PostId = post.Id,
            Title = post.Title,
            Body = post.Body,
            AuthorName = userRepository.GetSingleAsync(post.UserId).Result
                .Username
        };
        return Ok(getPostDto);
    }*/


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