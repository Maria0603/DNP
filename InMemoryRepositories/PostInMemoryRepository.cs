﻿using Entities;
using RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InMemoryRepositories;

public class PostInMemoryRepository : BaseInMemoryRepository<Post>,
    IPostRepository {
    private readonly IUserRepository userRepository;

    public PostInMemoryRepository(IUserRepository userRepository) {
        this.userRepository = userRepository;

        /*items = new List<Post> {
            new Post(1, "Post 1", "Body 1", 1),
            new Post(2, "Post 2", "Body 2", 1),
            new Post(3, "Post 3", "Body 3", 1),
            new Post(4, "Post 4", "Body 4", 1),
            new Post(5, "Post 5", "Body 5", 1)
        };*/
    }

    public override async Task<Post> AddAsync(Post post) {
        ValidatePost(post);
        return await base.AddAsync(post);
    }

    public override async Task UpdateAsync(Post post) {
        ValidatePost(post);
        await base.UpdateAsync(post);
    }

    private void ValidatePost(Post post) {
        if (string.IsNullOrWhiteSpace(post.Title)) {
            throw new InvalidOperationException("Post title is required");
        }

        if (string.IsNullOrWhiteSpace(post.Body)) {
            throw new InvalidOperationException("Post body is required");
        }

        if (items.Any(p => p.Id == post.Id)) {
            throw new InvalidOperationException(
                "Post with the same id already exists");
        }

        if (!userRepository.GetMany().Any(u => u.Id == post.UserId)) {
            throw new InvalidOperationException(
                "Post must be made by an existing user");
        }
    }
}