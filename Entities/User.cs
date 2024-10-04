using System;

namespace Entities;

public class User : IEntity {
    private static readonly Random random = new Random();

    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }

    public User(int id, string username, string password) {
        Id = id;
        Username = username;
        Password = password;
    }

    public User(string username, string password) : this(random.Next(1, int.MaxValue), username, password) {}

    public override string ToString() {
        return $"User {Id}: {Username}, {Password}";
    }
}