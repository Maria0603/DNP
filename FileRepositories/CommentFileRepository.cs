using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository {
    private readonly string _filePath;
    private List<Comment> _comments;

    public CommentFileRepository() {
        _filePath = "comments.json";
        if (!File.Exists(_filePath)) {
            _comments = new List<Comment> {
                new Comment {
                    Id = 1,
                    Body = "First Comment",
                    PostId = 1,
                    UserId = 1
                },
                new Comment {
                    Id = 2,
                    Body = "Second Comment",
                    PostId = 1,
                    UserId = 1
                }
            };
            SaveToFile();
        } else {
            LoadFromFile();
        }
    }

    public async Task<Comment> AddAsync(Comment comment) {
        comment.Id = _comments.Any() ? _comments.Max(c => c.Id) + 1 : 1;
        _comments.Add(comment);
        SaveToFile();
        return await Task.FromResult(comment);
    }

    public async Task UpdateAsync(Comment comment) {
        var index = _comments.FindIndex(c => c.Id == comment.Id);
        if (index == -1) {
            throw new InvalidOperationException($"Comment with id {comment.Id} not found");
        }
        _comments[index] = comment;
        SaveToFile();
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id) {
        var comment = _comments.FirstOrDefault(c => c.Id == id);
        if (comment == null) {
            throw new InvalidOperationException($"Comment with id {id} not found");
        }
        _comments.Remove(comment);
        SaveToFile();
        await Task.CompletedTask;
    }

    public async Task<Comment> GetSingleAsync(int id) {
        var comment = _comments.FirstOrDefault(c => c.Id == id);
        if (comment == null) {
            throw new InvalidOperationException($"Comment with id {id} not found");
        }
        return await Task.FromResult(comment);
    }

    public IQueryable<Comment> GetMany() {
        return _comments.AsQueryable();
    }

 public IQueryable<Comment> GetCommentsForPost(int postId) {
    return _comments.Where(c => c.PostId == postId).AsQueryable();
}

    private void SaveToFile() {
        string commentsJson = JsonSerializer.Serialize(_comments);
        File.WriteAllText(_filePath, commentsJson);
    }

    private void LoadFromFile() {
        string commentsJson = File.ReadAllText(_filePath);
        _comments = JsonSerializer.Deserialize<List<Comment>>(commentsJson) ?? new List<Comment>();
    }
}