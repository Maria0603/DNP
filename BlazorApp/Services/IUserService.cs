using System.Linq;
using System.Threading.Tasks;
using ApiContracts;

namespace BlazorApp.Services;

public interface IUserService {
    public Task<GetUserDto> AddUserAsync(CreateUserDto request);
    public IQueryable<GetUserDto> GetUsers(string? username);
    public Task<GetUserDto> GetUserAsync(int id);
    public Task DeleteUserAsync(int id);
    public Task UpdateUserAsync(int id, UpdateUserDto request);
    
}