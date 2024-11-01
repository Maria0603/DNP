using ApiContracts;

namespace BlazorApp.Services;

public interface IUserService {
    public Task<UserDto> AddUserAsync(CreateUserDto request);
    public IQueryable<UserDto> GetUsers(string? username);
    public Task<UserDto> GetUserAsync(int id);
    public Task DeleteUserAsync(int id);
    public Task UpdateUserAsync(int id, UpdateUserDto request);
    
}