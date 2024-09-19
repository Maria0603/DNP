﻿namespace Entities;

public class User (int id, string username, string password) : IEntity {
    public int Id { get; set; } = id;
    public string Username { get; set; } = username;
    public string Password { get; set; } = password;
    
    public override string ToString() {
        return $"User {Id}: {Username}, {Password}";
    }
}