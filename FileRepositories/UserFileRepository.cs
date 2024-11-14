using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository {
    private readonly string _filePath;
    private List<User> _users;

    public UserFileRepository() {
        _filePath = "users.json";
        if (!File.Exists(_filePath)) {
            _users = new List<User> {
                new User {
                    Id = 0,
                    Username = "John Doe",
                    Password = "password"
                },
                new User {
                    Id = 1,
                    Username = "Kane Koe",
                    Password = "password"
                }
            };
            SaveToFile();
        } else {
            LoadFromFile();
        }
    }

    public async Task<User> AddAsync(User user) {
        user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
        _users.Add(user);
        SaveToFile();
        return await Task.FromResult(user);
    }

    public async Task UpdateAsync(User user) {
        var index = _users.FindIndex(u => u.Id == user.Id);
        if (index == -1) {
            throw new InvalidOperationException($"User with id {user.Id} not found");
        }
        _users[index] = user;
        SaveToFile();
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id) {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) {
            throw new InvalidOperationException($"User with id {id} not found");
        }
        _users.Remove(user);
        SaveToFile();
        await Task.CompletedTask;
    }

    public async Task<User> GetSingleAsync(int id) {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) {
            throw new InvalidOperationException($"User with id {id} not found");
        }
        return await Task.FromResult(user);
    }

    public IQueryable<User> GetMany() {
        return _users.AsQueryable();
    }

    private void SaveToFile() {
        string usersJson = JsonSerializer.Serialize(_users);
        File.WriteAllText(_filePath, usersJson);
    }

    private void LoadFromFile() {
        string usersJson = File.ReadAllText(_filePath);
        _users = JsonSerializer.Deserialize<List<User>>(usersJson) ?? new List<User>();
    }
}