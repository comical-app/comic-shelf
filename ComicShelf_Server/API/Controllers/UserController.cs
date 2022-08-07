using API.Domain.Commands;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserRepository userRepository, ILogger<UserController> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }
    
    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        var user = _userRepository.GetUserByIdAsync(id);
        
        if (user == null)
            return NotFound();
        
        return Ok(user);
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        var users = _userRepository.ListUsersAsync();
        
        if (users == null)
            return NotFound();
        
        return Ok(users);
    }
    
    [HttpPost]
    public IActionResult Post([FromBody] CreateUserRequest user)
    {
        if (user == null)
            return BadRequest();
        
        _userRepository.CreateUserAsync(user);
        
        return Ok(user);
    }
    
    [HttpPut]
    public IActionResult Put([FromBody] UpdateUserRequest user)
    {
        if (user == null)
            return BadRequest();
        
        var checkUser = _userRepository.GetUserByIdAsync(user.Id);
        
        if (checkUser == null)
            return NotFound();
        
        _userRepository.UpdateUserAsync(user);
        
        return Ok(user);
    }
    
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var user = _userRepository.GetUserByIdAsync(id);
        
        if (user == null)
            return NotFound();
        
        _userRepository.DeleteUserAsync(id);
        
        return Ok(user);
    }
}