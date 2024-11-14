using System.Collections.Generic;
using System.Threading.Tasks;
using ApiContracts;

namespace BlazorApp.Services {
    public interface IPostService {
        Task<GetPostDto> CreatePostAsync(CreatePostDto request);
        Task<IEnumerable<GetPostDto>> GetPostsAsync();
        Task<GetPostDto> GetPostAsync(int postId);
        Task AddCommentAsync(CreateCommentDto request);
        
    }
}