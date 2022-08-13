using System;
using System.Threading.Tasks;
using FluentAssertions;
using Infra.Context;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.RepositoryInterfaces;
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
    public class GetUserByUsernameAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;
        private readonly Guid _userId;
        private readonly string _username;
        private readonly DateTime _createdAt;
        private readonly DateTime _updatedAt;
        private readonly DateTime _lastLogin;

        public GetUserByUsernameAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
            _userId = Guid.NewGuid();
            _username = "UserPowerAdmin";
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
                Username = _username,
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
                result.Username.Should().Be("UserPowerAdmin");
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
    public class CheckIfUsernameIsUniqueAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;

        public CheckIfUsernameIsUniqueAsync()
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
        public async Task Should_return_false_when_username_exists()
        {
            // Act
            var result = await _userRepository.CheckIfUsernameIsUniqueAsync("UserRegular");

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public async Task Should_return_true_when_username_does_not_exist()
        {
            // Act
            var result = await _userRepository.CheckIfUsernameIsUniqueAsync("UserDoesNotExist");

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task Should_throw_exception_when_username_is_empty()
        {
            // Act
            Func<Task> action = async () => await _userRepository.CheckIfUsernameIsUniqueAsync(string.Empty);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Test]
        public async Task Should_throw_exception_when_username_is_whitespace()
        {
            // Act
            Func<Task> action = async () => await _userRepository.CheckIfUsernameIsUniqueAsync(" ");

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
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
            var newUser = new User
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
            
            var newUser = new User
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
                Username = "UserAdminXYZ",
                IsAdmin = true,
                CanAccessOpds = true
            };

            // Act
            await _userRepository.UpdateUserAsync(user);
            var result = await _userRepository.GetUserByIdAsync(_userId);

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
    }

    [TestFixture]
    public class DeleteUserAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IUserRepository _userRepository;
        private readonly Guid _userId;
        private User _user;

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
            _user = new User
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
            await _dbContext.Users.AddAsync(_user);

            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task Should_delete_user()
        {
            // Act
            await _userRepository.DeleteUserAsync(_user);
            var result = await _userRepository.GetUserByIdAsync(_userId);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task Should_return_false_when_user_does_not_exist()
        {
            // Arrange
            var randomUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "UserAdminXYZ",
                IsAdmin = true,
                CanAccessOpds = true
            };
            
            // Act
            var result = await _userRepository.DeleteUserAsync(randomUser);

            // Assert
            result.Should().BeFalse();
        }
    }
}