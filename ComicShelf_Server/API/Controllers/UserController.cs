using API.Domain.Commands;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;

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

    /// <summary>
    /// Returns all users
    /// </summary>
    /// <response code="200">Users retrieved</response>
    /// <response code="204">No users</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<User>), 200)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Get()
    {
        var users = await _userRepository.ListUsersAsync();

        if (!users.Any()) return NoContent();

        return Ok(users);
    }
    
    /// <summary>
    /// Returns all users with OPDS access
    /// </summary>
    /// <response code="200">Users retrieved</response>
    /// <response code="204">No users</response>
    [HttpGet("opds-access")]
    [ProducesResponseType(typeof(IEnumerable<User>), 200)]
    [ProducesResponseType(typeof(IEnumerable<User>), 204)]
    public async Task<IActionResult> GetUsersWithOpdsAccess()
    {
        var users = await _userRepository.ListUsersWithOpdsAccessAsync();

        if (!users.Any()) return NoContent();

        return Ok(users);
    }

    /// <summary>
    /// Find user by id
    /// </summary>
    /// <param name="userId" example="e9a314af-d4b6-4907-a707-ca583571f596">User identification</param>
    /// <response code="200">User retrieved</response>
    /// <response code="404">User not found</response>
    [HttpGet("{userId:guid}")]
    [ProducesResponseType(typeof(User), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Get(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    /// <summary>
    /// Check if username exists
    /// </summary>
    /// <param name="username" example="johndoe">Username</param>
    /// <response code="200">Username exists</response>
    /// <response code="500">Fail to check if the username exists</response>
    [HttpGet("verify-username/{username}")]
    [ProducesResponseType(typeof(bool), 200)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CheckUsername(string username)
    {
        try
        {
            var user = await _userRepository.CheckIfUsernameIsUniqueAsync(username);

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to check if the username exists");
        }
    }

    /// <summary>
    /// Add new user
    /// </summary>
    /// <param name="user">User object that needs to be added</param>
    /// <response code="201">User added</response>
    /// <response code="500">Fail to create user</response>
    [HttpPost]
    [ProducesResponseType(typeof(User), 201)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Post([FromBody] CreateUserRequest user)
    {
        try
        {
            var result = await _userRepository.CreateUserAsync(user);

            return Created($"/user/{result.Id}", result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to create user");
        }
    }

    /// <summary>
    /// Update an user
    /// </summary>
    /// <param name="userId" example="e9a314af-d4b6-4907-a707-ca583571f596">User identification</param>
    /// <param name="user">User object that needs to be updated</param>
    /// <response code="204">User updated</response>
    /// <response code="404">User not found</response>
    /// <response code="500">Fail to update user</response>
    [HttpPut("{userId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Put(Guid userId, [FromBody] UpdateUserRequest user)
    {
        try
        {
            var checkUser = await _userRepository.GetUserByIdAsync(userId);

            if (checkUser == null)
                return NotFound();

            var result = await _userRepository.UpdateUserAsync(userId, user);

            if (result) return NoContent();

            return BadRequest("Fail to update user");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to update user");
        }
    }

    /// <summary>
    /// Delete an user
    /// </summary>
    /// <param name="userId" example="e9a314af-d4b6-4907-a707-ca583571f596">User identification</param>
    /// <response code="204">User deleted</response>
    /// <response code="404">User not found</response>
    /// <response code="500">Fail to delete user</response>
    [HttpDelete("{userId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> Delete(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound();

            var result = await _userRepository.DeleteUserAsync(userId);

            if (result) return NoContent();

            return BadRequest("Fail to delete user");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to delete user");
        }
    }

    /// <summary>
    /// Change user's password
    /// </summary>
    /// <response code="204">User's password changed</response>
    /// <response code="404">User not found</response>
    /// <response code="500">Fail to change user password</response>
    [HttpPost("change-password")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangeUserPasswordRequest request)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            if (user == null)
                return NotFound();

            var result = await _userRepository.ChangePasswordAsync(request);

            if (result) return NoContent();

            return BadRequest("Fail to change user password");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to change user password");
        }
    }

    /// <summary>
    /// Reset user's password
    /// </summary>
    /// <response code="204">User's password reseted</response>
    /// <response code="404">User not found</response>
    /// <response code="500">Fail to reset user password</response>
    [HttpPost("reset-password")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ResetUserPasswordRequest request)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(request.UserId);

            if (user == null)
                return NotFound();

            var result = await _userRepository.ResetPasswordAsync(request);

            if (result) return NoContent();

            return BadRequest("Fail to reset user password");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to reset user password");
        }
    }
    
    /// <summary>
    /// Activate a user
    /// </summary>
    /// <response code="204">User activated</response>
    /// <response code="404">User not found</response>
    /// <response code="500">Fail to activate user</response>
    [HttpPost("activate-user")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> ActivateUser([FromBody] Guid userId)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound();

            var result = await _userRepository.ActivateUserAsync(userId);

            if (result) return NoContent();

            return BadRequest("Fail to activate user");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to activate user");
        }
    }

    /// <summary>
    /// Deactivate a user
    /// </summary>
    /// <response code="204">User deactivated</response>
    /// <response code="404">User not found</response>
    /// <response code="500">Fail to deactivate user</response>
    [HttpPost("deactivate-user")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeactivateUser([FromBody] Guid userId)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound();

            var result = await _userRepository.DeactivateUserAsync(userId);

            if (result) return NoContent();

            return BadRequest("Fail to deactivate user");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to deactivate user");
        }
    }
    
    /// <summary>
    /// Give ODPS access to a user
    /// </summary>
    /// <response code="204">Permission granted</response>
    /// <response code="404">User not found</response>
    /// <response code="500">Fail to give ODPS access</response>
    [HttpPost("give-odps-access")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GiveOdpsAccess([FromBody] Guid userId)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound();

            var result = await _userRepository.GiveOpdsAccessAsync(userId);

            if (result) return NoContent();

            return BadRequest("Fail to give odps access");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to give odps access");
        }
    }
    
    /// <summary>
    /// Take ODPS access
    /// </summary>
    /// <response code="204">Permission removed</response>
    /// <response code="404">User not found</response>
    /// <response code="500">Fail to take ODPS access</response>
    [HttpPost("take-odps-access")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> TakeOpdsAccess([FromBody] Guid userId)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound();

            var result = await _userRepository.TakeOpdsAccessAsync(userId);

            if (result) return NoContent();

            return BadRequest("Fail to take ODPS access");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest("Fail to take ODPS access");
        }
    }
}