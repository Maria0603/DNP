using System;

namespace Entities;

public class Post : IEntity {
    private static readonly Random random = new Random();

    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }

    public Post(int id, string title, string body, int userId) {
        Id = id;
        Title = title;
        Body = body;
        UserId = userId;
    }

    public Post(string title, string body, int userId)
        : this(random.Next(1, int.MaxValue), title, body, userId) {}

    public override string ToString() {
        return $"Post {Id}: {Title}, {Body}, UserId: {UserId}";
    }
}