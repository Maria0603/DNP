using Entities;
using RepositoryContracts;
using System.Collections.Generic;

namespace InMemoryRepositories;

public class UserInMemoryRepository : BaseInMemoryRepository<User>,
    IUserRepository {
    public UserInMemoryRepository() : base(new List<User> {
        new User { Id = 1, Username = "User 1", Password = "user1@example.com" },
        new User { Id = 2, Username = "User 2", Password = "user2@example.com" },
        new User { Id = 3, Username = "User 3", Password = "user3@example.com" },
        new User { Id = 4, Username = "User 4", Password = "user4@example.com" },
        new User { Id = 5, Username = "User 5", Password = "user5@example.com" }
    }) {
    }
}