using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using ApiContracts;
using Entities;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserRepository userRepository) : ControllerBase {
    
    
    [HttpPost]
    public async Task<ActionResult<GetUserDto>> AddUserAsync([FromBody] CreateUserDto request) {
        Console.WriteLine("Username: " + request.Username + "; Password: " + request.Password);
        User user = new User {
            Username = request.Username,
            Password = request.Password
        };
        User createdUser = await userRepository.AddAsync(user);
        GetUserDto getUserDto = new() {
            Id = createdUser.Id,
            Username = createdUser.Username
        };
        return Created($"/Users", getUserDto); 
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<GetUserDto>> GetUsers([FromQuery] string? username) {
        IEnumerable<User> users = userRepository.GetMany();
        
        if (username != null) {
            users = users.Where(user => user.Username.Contains(username));
        }
        IEnumerable<GetUserDto> userDtos = users.Select(user => new GetUserDto {
            Id = user.Id,
            Username = user.Username
        });
        return Ok(userDtos);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<GetUserDto>> GetUser(int id) {
        User user = await userRepository.GetSingleAsync(id);
        GetUserDto getUserDto = new() {
            Id = user.Id,
            Username = user.Username
        };
        return Ok(getUserDto);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id) {
        await userRepository.DeleteAsync(id);
        return NoContent();
    }
    
    
}