using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IUserRepository userRepository) : ControllerBase {
    
    [HttpPost("login")]
    public async Task<ActionResult<GetUserDto>> Login([FromBody] LoginRequestDto request)
    {
        IQueryable<User> users = userRepository.GetMany();
        
        User user = users.SingleOrDefault(u => u.Username == request.Username);

        if (user is null)
            return Unauthorized("Username is incorrect");
        
        if(!user.Password.Equals(request.Password))
            return Unauthorized("Password is incorrect");
        
        GetUserDto sendDto = new GetUserDto{Username = user.Username, Id = user.Id};
        return sendDto;
    }
    
}