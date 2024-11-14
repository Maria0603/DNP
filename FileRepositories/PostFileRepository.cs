using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : IPostRepository {
    private readonly string _filePath;
    private List<Post> _posts;

    public PostFileRepository() {
        _filePath = "posts.json";
        if (!File.Exists(_filePath)) {
            _posts = new List<Post> {
                new Post {
                    Id = 1,
                    Title = "First Post",
                    Body = "This is the first post.",
                    UserId = 1
                },
                new Post {
                    Id = 2,
                    Title = "Second Post",
                    Body = "This is the second post.",
                    UserId = 2
                }
            };
            SaveToFile();
        } else {
            LoadFromFile();
        }
    }

    public async Task<Post> AddAsync(Post post) {
        post.Id = _posts.Any() ? _posts.Max(p => p.Id) + 1 : 1;
        _posts.Add(post);
        SaveToFile();
        return await Task.FromResult(post);
    }

    public async Task UpdateAsync(Post post) {
        var index = _posts.FindIndex(p => p.Id == post.Id);
        if (index == -1) {
            throw new InvalidOperationException($"Post with id {post.Id} not found");
        }
        _posts[index] = post;
        SaveToFile();
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id) {
        var post = _posts.FirstOrDefault(p => p.Id == id);
        if (post == null) {
            throw new InvalidOperationException($"Post with id {id} not found");
        }
        _posts.Remove(post);
        SaveToFile();
        await Task.CompletedTask;
    }

    public async Task<Post> GetSingleAsync(int id) {
        var post = _posts.FirstOrDefault(p => p.Id == id);
        if (post == null) {
            throw new InvalidOperationException($"Post with id {id} not found");
        }
        return await Task.FromResult(post);
    }

    public IQueryable<Post> GetMany() {
        return _posts.AsQueryable();
    }

    private void SaveToFile() {
        string postsJson = JsonSerializer.Serialize(_posts);
        File.WriteAllText(_filePath, postsJson);
    }

    private void LoadFromFile() {
        string postsJson = File.ReadAllText(_filePath);
        _posts = JsonSerializer.Deserialize<List<Post>>(postsJson) ?? new List<Post>();
    }
}