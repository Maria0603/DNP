using Entities;
using RepositoryContracts;
using System.Collections.Generic;

namespace InMemoryRepositories;

public class PostInMemoryRepository : BaseInMemoryRepository<Post>,
    IPostRepository {
    public PostInMemoryRepository() : base(new List<Post> {
        new Post { Id = 1, Title = "Post 1", Body = "Content 1", UserId = 1 },
        new Post { Id = 2, Title = "Post 2", Body = "Content 2", UserId = 2 },
        new Post { Id = 3, Title = "Post 3", Body = "Content 3", UserId = 3 },
        new Post { Id = 4, Title = "Post 4", Body = "Content 4", UserId = 4 },
        new Post { Id = 5, Title = "Post 5", Body = "Content 5", UserId = 5 }
    }) {
    }
}