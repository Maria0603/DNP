using System;

namespace Entities;

public class Post()
    : IEntity {
    private static readonly Random random = new Random();

    public int Id { get; set; }
    public string Title { get; set; } 
    public string Body { get; set; } 
    public int UserId { get; set; } 

    /*public Post(string title, string body, int userId)
        : this(random.Next(1, int.MaxValue), title, body, userId) {}*/

    public override string ToString() {
        return $"Post {Id}: {Title}, {Body}, UserId: {UserId}";
    }
}