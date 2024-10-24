using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository : BaseFileRepository<Post>, IPostRepository {
    public PostFileRepository() : base("posts.json") {
        if (!File.Exists("posts.json")) {
            var dummyPosts = new List<Post> {
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
            string postsJson = JsonSerializer.Serialize(dummyPosts);
            File.WriteAllText("posts.json", postsJson);
        }
    }
}