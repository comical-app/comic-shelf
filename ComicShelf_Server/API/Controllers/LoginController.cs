using API.Domain.Commands;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<LoginController> _logger;

    public LoginController(IUserRepository userRepository, ILogger<LoginController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }
    
    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequest user)
    {
        var userLogin = await _userRepository.LoginAsync(user);
        
        if (userLogin == null)
        {
            return BadRequest(new { message = "User or password invalid" });
        }
        
        return Ok(userLogin);
    }
}