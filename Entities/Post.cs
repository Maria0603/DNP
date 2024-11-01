using System;

namespace Entities;

public class Post()
    : IEntity {
    private static readonly Random random = new Random();

    public int Id { get; set; }
    public string Title { get; init; } 
    public string Body { get; init; } 
    public int UserId { get; init; } 



    public override string ToString() {
        return $"Post {Id}: {Title}, {Body}, UserId: {UserId}";
    }
}