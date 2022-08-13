using Microsoft.AspNetCore.Mvc;
using Models.Commands;
using Models.Domain;
using Models.ServicesInterfaces;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<LoginController> _logger;

    public LoginController(IUserService userService, ILogger<LoginController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <response code="200">User logged</response>
    /// <response code="500">User or password invalid</response>
    /// <response code="500">An error occurred while trying to login</response>
    [HttpPost]
    [ProducesResponseType(typeof(User), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserCommand user)
    {
        try
        {
            var userLogin = await _userService.LoginAsync(user);

            if (userLogin == null) return BadRequest("User or password invalid");

            return Ok(userLogin);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("An error occurred while trying to login");
        }
    }
}