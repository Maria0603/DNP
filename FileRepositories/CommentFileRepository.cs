using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : BaseFileRepository<Comment>, ICommentRepository {
    public CommentFileRepository() : base("comments.json") {
        if (!File.Exists("comments.json")) {
            var dummyComments = new List<Comment> {
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
            string commentsJson = JsonSerializer.Serialize(dummyComments);
            File.WriteAllText("comments.json", commentsJson);
        }
    }
}