using System;

namespace Entities;

public class User()
    : IEntity {
    private static readonly Random random = new Random();

    public int Id { get; set; } 
    public string Username { get; set; } 
    public string Password { get; set; } 
    

    public override string ToString() {
        return $"User {Id}: {Username}, {Password}";
    }
}