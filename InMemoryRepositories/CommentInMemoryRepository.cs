using Entities;
using RepositoryContracts;
using System.Collections.Generic;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : BaseInMemoryRepository<Comment>,
    ICommentRepository {
    public CommentInMemoryRepository() {
        items = new List<Comment> {
            new Comment { Id = 1, Body = "Comment 1" },
            new Comment { Id = 2, Body = "Comment 2" },
            new Comment { Id = 3, Body = "Comment 3" },
            new Comment { Id = 4, Body = "Comment 4" },
            new Comment { Id = 5, Body = "Comment 5" }
        };
    }
}