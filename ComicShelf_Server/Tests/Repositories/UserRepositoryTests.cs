using System;
using System.Threading.Tasks;
using API.Context;
using API.Domain.Commands;
using API.Interfaces;
using API.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Models;
using NUnit.Framework;

namespace Tests.Repositories;

[TestFixture]
public class UserRepositoryTests
{
    [TestFixture]
    public class ListUsersAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;

        public ListUsersAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
        }

        [SetUp]
        public async Task Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);

            // Create admin user
            var user1 = new User
            {
                Id = Guid.NewGuid(),
                Username = "UserAdmin",
                Password = "12345",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = true
            };
            await _dbContext.Users.AddAsync(user1);

            // Create regular user
            var user2 = new User
            {
                Id = Guid.NewGuid(),
                Username = "UserRegular",
                Password = "123456",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = true
            };
            await _dbContext.Users.AddAsync(user2);

            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task Should_return_all_users()
        {
            // Act
            var users = await _userRepository.ListUsersAsync();

            // Assert
            users.Should().HaveCount(2);
        }
    }

    [TestFixture]
    public class ListUsersWithOpdsAccessAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;

        public ListUsersWithOpdsAccessAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
        }

        [SetUp]
        public async Task Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);

            // Create admin user
            var user1 = new User
            {
                Id = Guid.NewGuid(),
                Username = "UserAdmin",
                Password = "12345",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = true
            };
            await _dbContext.Users.AddAsync(user1);

            // Create regular user
            var user2 = new User
            {
                Id = Guid.NewGuid(),
                Username = "UserRegular",
                Password = "123456",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = false
            };
            await _dbContext.Users.AddAsync(user2);

            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task Should_return_all_users_with_admin_role()
        {
            // Act
            var users = await _userRepository.ListUsersWithOpdsAccessAsync();

            // Assert
            users.Should().HaveCount(1);
        }
    }

    [TestFixture]
    public class GetUserByIdAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;
        private readonly Guid _userId;
        private readonly DateTime _createdAt;
        private readonly DateTime _updatedAt;
        private readonly DateTime _lastLogin;

        public GetUserByIdAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
            _userId = Guid.NewGuid();
            _createdAt = DateTime.Today;
            _updatedAt = DateTime.Today;
            _lastLogin = DateTime.Today;
        }

        [SetUp]
        public void Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
        }

        [Test]
        public async Task Should_return_valid_user()
        {
            // Arrange
            var user1 = new User
            {
                Id = _userId,
                Username = "UserAdmin",
                Password = "12345",
                CreatedAt = _createdAt,
                UpdatedAt = _updatedAt,
                IsActive = true,
                IsAdmin = true,
                LastLogin = _lastLogin,
                CanAccessOpds = true
            };
            await _dbContext.Users.AddAsync(user1);

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetUserByIdAsync(_userId);

            // Assert
            result.Should().NotBeNull();
            if (result != null)
            {
                result.Id.Should().Be(_userId);
                result.Username.Should().Be("UserAdmin");
                result.Password.Should().Be("12345");
                result.CreatedAt.Should().Be(_createdAt);
                result.UpdatedAt.Should().Be(_updatedAt);
                result.IsActive.Should().BeTrue();
                result.IsAdmin.Should().BeTrue();
                result.LastLogin.Should().Be(_lastLogin);
                result.CanAccessOpds.Should().BeTrue();
            }
        }

        [Test]
        public async Task Should_return_null_when_user_not_found()
        {
            // Act
            var result = await _userRepository.GetUserByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }
    }

    [TestFixture]
    public class CheckIfUsernameExistsAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;

        public CheckIfUsernameExistsAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
        }

        [SetUp]
        public async Task Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);

            // Create admin user
            var user1 = new User
            {
                Id = Guid.NewGuid(),
                Username = "UserAdmin",
                Password = "12345",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = true
            };
            await _dbContext.Users.AddAsync(user1);

            // Create regular user
            var user2 = new User
            {
                Id = Guid.NewGuid(),
                Username = "UserRegular",
                Password = "123456",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = false
            };
            await _dbContext.Users.AddAsync(user2);

            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task Should_return_true_when_username_exists()
        {
            // Act
            var result = await _userRepository.CheckIfUsernameExistsAsync("UserRegular");

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task Should_return_false_when_username_does_not_exist()
        {
            // Act
            var result = await _userRepository.CheckIfUsernameExistsAsync("UserDoesNotExist");

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public async Task Should_return_false_when_username_is_empty()
        {
            // Act
            var result = await _userRepository.CheckIfUsernameExistsAsync(string.Empty);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public async Task Should_return_false_when_username_is_whitespace()
        {
            // Act
            var result = await _userRepository.CheckIfUsernameExistsAsync(" ");

            // Assert
            result.Should().BeFalse();
        }
    }

    [TestFixture]
    public class CreateUserAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;

        public CreateUserAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
        }

        [SetUp]
        public Task Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
            return Task.CompletedTask;
        }

        [Test]
        public async Task Should_create_user()
        {
            // Arrange
            var newUser = new CreateUserRequest()
            {
                Username = "UserAdmin",
                Password = "12345",
                IsAdmin = true,
                CanAccessOpds = true
            };

            // Act
            var user = await _userRepository.CreateUserAsync(newUser);
            var result = await _userRepository.GetUserByIdAsync(user.Id);

            // Assert
            result.Should().NotBeNull();
            if (result != null)
            {
                result.Id.Should().Be(user.Id);
                result.Username.Should().Be(newUser.Username);
                result.Password.Should().Be(newUser.Password);
                result.IsAdmin.Should().Be(newUser.IsAdmin);
                result.CanAccessOpds.Should().Be(newUser.CanAccessOpds);
                result.IsActive.Should().BeTrue();
            }
        }

        [Test]
        public async Task Should_throw_exception_when_user_already_exists()
        {
            // Arrange
            var user = new User
            {
                Username = "UserAdmin",
                Password = "12345",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = true
            };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            
            var newUser = new CreateUserRequest()
            {
                Username = user.Username,
                Password = user.Password,
                IsAdmin = true,
                CanAccessOpds = true
            };

            // Act
            Func<Task> result = async () =>
            {
                await _userRepository.CreateUserAsync(newUser);
            };

            // Assert
            await result.Should().ThrowAsync<Exception>();
        }

        [Test]
        public async Task Should_throw_exception_when_user_is_null()
        {
            // Act
            Func<Task> result = async () => { await _userRepository.CreateUserAsync(null); };

            // Assert
            await result.Should().ThrowAsync<Exception>();
        }
    }

    [TestFixture]
    public class UpdateUserAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;
        private readonly Guid _userId;

        public UpdateUserAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;

            _userId = Guid.NewGuid();
        }

        [SetUp]
        public void Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
        }

        [Test]
        public async Task Should_update_user()
        {
            // Arrange
            var existentUser = new User
            {
                Id = _userId,
                Username = "UserAdmin",
                Password = "12345",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = true
            };
            await _dbContext.Users.AddAsync(existentUser);

            await _dbContext.SaveChangesAsync();
            
            var user = new User
            {
                Id = _userId,
                Username = "UserAdminXYZ",
                Password = "12345",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = true
            };

            // Act
            await _userRepository.UpdateUserAsync(user);
            var result = await _userRepository.GetUserByIdAsync(user.Id);

            // Assert
            result.Should().NotBeNull();
            if (result != null)
            {
                result.Id.Should().Be(_userId);
                result.Username.Should().Be("UserAdminXYZ");
                result.Password.Should().Be("12345");
                result.IsActive.Should().BeTrue();
                result.IsAdmin.Should().BeTrue();
                result.CanAccessOpds.Should().BeTrue();
            }
        }
        
        [Test]
        public async Task Should_throw_exception_when_user_is_null()
        {
            // Act
            Func<Task> result = async () => { await _userRepository.UpdateUserAsync(null); };

            // Assert
            await result.Should().ThrowAsync<Exception>();
        }
        
        [Test]
        public async Task Should_throw_exception_when_user_does_not_exist()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "UserAdminXYZ",
                Password = "12345",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = true
            };

            // Act
            var result =  await _userRepository.UpdateUserAsync(user);

            // Assert
            result.Should().BeFalse();
        }
    }

    [TestFixture]
    public class DeleteUserAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;
        private readonly Guid _userId;

        public DeleteUserAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
            _userId = Guid.NewGuid();
        }

        [SetUp]
        public async Task Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);

            // Create a user
            var user1 = new User
            {
                Id = _userId,
                Username = "UserAdmin",
                Password = "12345",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = true
            };
            await _dbContext.Users.AddAsync(user1);

            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task Should_delete_user()
        {
            // Act
            await _userRepository.DeleteUserAsync(_userId);
            var result = await _userRepository.GetUserByIdAsync(_userId);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task Should_return_false_when_user_does_not_exist()
        {
            // Act
            var result = await _userRepository.DeleteUserAsync(Guid.NewGuid());

            // Assert
            result.Should().BeFalse();
        }
    }

    [TestFixture]
    public class LoginAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;
        private readonly Guid _userId;
        private readonly string _username;
        private readonly string _password;
        private readonly bool _isAdmin;
        private readonly bool _canAccessOpds;

        public LoginAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;

            _userId = Guid.NewGuid();
            _username = "UserAdmin";
            _password = "12345";
            _isAdmin = true;
            _canAccessOpds = true;
        }

        [SetUp]
        public void Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
        }

        [Test]
        public async Task Should_return_user_when_username_and_password_are_correct()
        {
            // Arrange

            // Create admin user
            var user1 = new User
            {
                Id = _userId,
                Username = _username,
                Password = _password,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = _isAdmin,
                LastLogin = DateTime.Now,
                CanAccessOpds = _canAccessOpds
            };
            await _dbContext.Users.AddAsync(user1);

            // Create regular user
            var user2 = new User
            {
                Id = Guid.NewGuid(),
                Username = "UserRegular",
                Password = "123456",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = false
            };
            await _dbContext.Users.AddAsync(user2);

            await _dbContext.SaveChangesAsync();
            
            const string username = "UserAdmin";
            const string password = "12345";

            // Act
            var result = await _userRepository.LoginAsync(username, password);

            // Assert
            result.Should().NotBeNull();
            if (result != null)
            {
                result.Id.Should().Be(_userId);
                result.Username.Should().Be(_username);
                result.Password.Should().Be(_password);
                result.IsActive.Should().BeTrue();
                result.IsAdmin.Should().Be(_isAdmin);
                result.CanAccessOpds.Should().Be(_canAccessOpds);
            }
        }

        [Test]
        public async Task Should_return_null_when_username_is_incorrect()
        {
            // Arrange
            var user = new User
            {
                Username = "UserAdmin",
                Password = "12345"
            };

            // Act
            var result = await _userRepository.LoginAsync("UserAdmin2", user.Password);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task Should_return_null_when_password_is_incorrect()
        {
            // Arrange
            var user = new User
            {
                Username = "UserAdmin",
                Password = "12345"
            };

            // Act
            var result = await _userRepository.LoginAsync(user.Username, "1234567");

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task Should_return_null_when_username_is_null()
        {
            // Arrange
            var user = new User
            {
                Username = "UserAdmin",
                Password = "12345"
            };

            // Act
            var result = await _userRepository.LoginAsync(null, user.Password);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task Should_return_null_when_password_is_null()
        {
            // Arrange
            var user = new User
            {
                Username = "UserAdmin",
                Password = "12345"
            };

            // Act
            var result = await _userRepository.LoginAsync(user.Username, null);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task Should_return_null_when_username_is_empty()
        {
            // Arrange
            var user = new User
            {
                Username = "UserAdmin",
                Password = "12345"
            };

            // Act
            var result = await _userRepository.LoginAsync("", user.Password);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task Should_return_null_when_password_is_empty()
        {
            // Arrange
            var user = new User
            {
                Username = "UserAdmin",
                Password = "12345"
            };

            // Act
            var result = await _userRepository.LoginAsync(user.Username, "");

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task Should_return_null_when_username_is_whitespace()
        {
            // Arrange
            var user = new User
            {
                Username = "UserAdmin",
                Password = "12345"
            };

            // Act
            var result = await _userRepository.LoginAsync(" ", user.Password);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task Should_return_null_when_password_is_whitespace()
        {
            // Arrange
            var user = new User
            {
                Username = "UserAdmin",
                Password = "12345"
            };

            // Act
            var result = await _userRepository.LoginAsync(user.Username, " ");

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task Should_return_null_when_username_is_null_and_password_is_null()
        {
            // Arrange
            var user = new User
            {
                Username = "UserAdmin",
                Password = "12345"
            };

            // Act
            var result = await _userRepository.LoginAsync(null, null);

            // Assert
            result.Should().BeNull();
        }
    }

    [TestFixture]
    public class ResetPasswordAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;
        private readonly Guid _userId;

        public ResetPasswordAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;

            _userId = Guid.NewGuid();
        }

        [SetUp]
        public void Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
        }

        [Test]
        public async Task Should_reset_password_when_user_exists()
        {
            // Arrange
            var user = new User
            {
                Id = _userId,
                Username = "UserAdmin",
                Password = "12345",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = true
            };
            await _dbContext.Users.AddAsync(user);

            await _dbContext.SaveChangesAsync();
            
            const string newPassword = "987654321";

            // Act
            var result = await _userRepository.ResetPasswordAsync(_userId, newPassword, newPassword);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task Should_return_false_when_user_does_not_exist()
        {
            // Arrange
            const string newPassword = "987654321";

            // Act
            var result = await _userRepository.ResetPasswordAsync(Guid.NewGuid(), newPassword, newPassword);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public async Task Should_return_false_when_new_password_is_null()
        {
            // Arrange
            const string newPassword = null;

            // Act
            Func<Task> result = async () => { await _userRepository.ResetPasswordAsync(_userId, newPassword, newPassword); };

            // Assert
            await result.Should().ThrowAsync<Exception>();
        }

        [Test]
        public async Task Should_return_false_when_new_password_is_empty()
        {
            // Arrange
            const string newPassword = "";

            // Act
            Func<Task> result = async () => { await _userRepository.ResetPasswordAsync(_userId, newPassword, newPassword); };

            // Assert
            await result.Should().ThrowAsync<Exception>();
        }

        [Test]
        public async Task Should_return_false_when_new_password_is_whitespace()
        {
            // Arrange
            const string newPassword = " ";

            // Act
            Func<Task> result = async () => { await _userRepository.ResetPasswordAsync(_userId, newPassword, newPassword); };

            // Assert
            await result.Should().ThrowAsync<Exception>();
        }

        [Test]
        public async Task Should_return_false_when_new_password_is_null_and_confirm_password_is_null()
        {
            // Arrange
            const string newPassword = null;
            const string confirmPassword = null;

            // Act
            Func<Task> result = async () => { await _userRepository.ResetPasswordAsync(_userId, newPassword, confirmPassword); };

            // Assert
            await result.Should().ThrowAsync<Exception>();
        }

        [Test]
        public async Task Should_return_false_when_new_password_is_empty_and_confirm_password_is_empty()
        {
            // Arrange
            const string newPassword = "";
            const string confirmPassword = "";

            // Act
            Func<Task> result = async () => { await _userRepository.ResetPasswordAsync(_userId, newPassword, confirmPassword); };

            // Assert
            await result.Should().ThrowAsync<Exception>();
        }

        [Test]
        public async Task Should_return_exception_when_password_doesnt_match()
        {
            // Arrange
            const string newPassword1 = "987654321";
            const string newPassword2 = "987654322";

            // Act
            Func<Task> action = async () =>
                await _userRepository.ResetPasswordAsync(_userId, newPassword1, newPassword2);

            // Assert
            await action.Should().ThrowAsync<Exception>();
        }
    }

    [TestFixture]
    public class ChangePasswordAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;
        private readonly Guid _userId;
        private readonly string _oldPassword;

        public ChangePasswordAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;

            _userId = Guid.NewGuid();
            _oldPassword = "12345";
        }

        [SetUp]
        public async Task Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
        }

        [Test]
        public async Task Should_change_password_when_user_exists()
        {
            // Arrange
            var user = new User
            {
                Id = _userId,
                Username = "UserRegular",
                Password = _oldPassword,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = false
            };
            await _dbContext.Users.AddAsync(user);

            await _dbContext.SaveChangesAsync();
            
            const string newPassword = "987654321";

            // Act
            var result = await _userRepository.ChangePasswordAsync(_userId, _oldPassword, newPassword, newPassword);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task Should_return_false_when_user_does_not_exist()
        {
            // Arrange
            const string newPassword = "987654321";

            // Act
            var result =
                await _userRepository.ChangePasswordAsync(Guid.NewGuid(), _oldPassword, newPassword, newPassword);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public async Task Should_return_false_when_old_password_is_wrong()
        {
            // Arrange
            const string newPassword = "987654321";

            // Act
            var result = await _userRepository.ChangePasswordAsync(_userId, "wrong", newPassword, newPassword);

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public async Task Should_return_false_when_new_password_is_null()
        {
            // Arrange
            const string newPassword = null;

            // Act
            Func<Task> action = async () =>
                await _userRepository.ChangePasswordAsync(_userId, _oldPassword, newPassword, newPassword);

            // Assert
            await action.Should().ThrowAsync<Exception>();
        }

        [Test]
        public async Task Should_return_exception_when_password_doesnt_match()
        {
            // Arrange
            const string newPassword1 = "987654321";
            const string newPassword2 = "987654322";

            // Act
            Func<Task> action = async () =>
                await _userRepository.ChangePasswordAsync(_userId, _oldPassword, newPassword1, newPassword2);

            // Assert
            await action.Should().ThrowAsync<Exception>();
        }
    }

    [TestFixture]
    public class ActivateUserAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;
        private readonly Guid _userId;

        public ActivateUserAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;

            _userId = Guid.NewGuid();
        }

        [SetUp]
        public async Task Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
        }

        [Test]
        public async Task Should_activate_user_when_user_exists()
        {
            // Arrange
            var user = new User
            {
                Id = _userId,
                Username = "UserRegular",
                Password = "12345",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = false
            };
            await _dbContext.Users.AddAsync(user);

            await _dbContext.SaveChangesAsync();
            
            // Act
            var result = await _userRepository.ActivateUserAsync(_userId);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task Should_return_false_when_user_does_not_exist()
        {
            // Act
            var result = await _userRepository.ActivateUserAsync(Guid.NewGuid());

            // Assert
            result.Should().BeFalse();
        }
    }

    [TestFixture]
    public class DeactivateUserAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;
        private readonly Guid _userId;

        public DeactivateUserAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;

            _userId = Guid.NewGuid();
        }

        [SetUp]
        public async Task Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
        }

        [Test]
        public async Task Should_deactivate_user_when_user_exists()
        {
            // Arrange
            var user = new User
            {
                Id = _userId,
                Username = "UserRegular",
                Password = "12345",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = false
            };
            await _dbContext.Users.AddAsync(user);

            await _dbContext.SaveChangesAsync();
            
            // Act
            var result = await _userRepository.DeactivateUserAsync(_userId);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task Should_return_false_when_user_does_not_exist()
        {
            // Act
            var result = await _userRepository.DeactivateUserAsync(Guid.NewGuid());

            // Assert
            result.Should().BeFalse();
        }
    }

    [TestFixture]
    public class GiveOpdsAccessAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;
        private readonly Guid _userId;

        public GiveOpdsAccessAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;

            _userId = Guid.NewGuid();
        }

        [SetUp]
        public async Task Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
        }

        [Test]
        public async Task Should_give_opds_access_when_user_exists()
        {
            // Arrange
            var user = new User
            {
                Id = _userId,
                Username = "UserRegular",
                Password = "12345",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = false
            };
            await _dbContext.Users.AddAsync(user);

            await _dbContext.SaveChangesAsync();
            
            // Act
            var result = await _userRepository.GiveOpdsAccessAsync(_userId);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task Should_return_false_when_user_does_not_exist()
        {
            // Act
            var result = await _userRepository.GiveOpdsAccessAsync(Guid.NewGuid());

            // Assert
            result.Should().BeFalse();
        }
    }

    [TestFixture]
    public class TakeOpdsAccessAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;
        private readonly Guid _userId;

        public TakeOpdsAccessAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;

            _userId = Guid.NewGuid();
        }

        [SetUp]
        public void Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
        }

        [Test]
        public async Task Should_take_opds_access_when_user_exists()
        {
            // Arrange
            var user = new User
            {
                Id = _userId,
                Username = "UserRegular",
                Password = "12345",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsAdmin = true,
                LastLogin = DateTime.Now,
                CanAccessOpds = false
            };
            await _dbContext.Users.AddAsync(user);

            await _dbContext.SaveChangesAsync();
            
            // Act
            var result = await _userRepository.TakeOpdsAccessAsync(_userId);

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task Should_return_false_when_user_does_not_exist()
        {
            // Act
            var result = await _userRepository.TakeOpdsAccessAsync(Guid.NewGuid());

            // Assert
            result.Should().BeFalse();
        }
    }
}