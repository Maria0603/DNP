using System;

namespace Entities;

public class Comment()
    : IEntity {
    private static readonly Random random = new Random();

    public int Id { get; set; } 
    public string Body { get; set; } 
    public int PostId { get; set; } 
    public int UserId { get; set; } 

    /*public Comment(string body, int postId, int userId) 
        : this(random.Next(1, int.MaxValue), body, postId, userId) {}*/

    public override string ToString() {
        return $"Comment {Id}: {Body}, PostId: {PostId}, UserId: {UserId}";
    }
}