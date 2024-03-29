using System;
using System.Linq;
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
public class LibraryRepositoryTests
{
    [TestFixture]
    public class ListLibrariesAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private ILibraryRepository _libraryRepository;

        public ListLibrariesAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
        }

        [SetUp]
        public async Task Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _libraryRepository = new LibraryRepository(_dbContext);

            var library1 = new Library
            {
                Name = "Library 1",
                Folders = new []{new LibraryFolder {Id = Guid.NewGuid(), Path = @"C:\Library 1"}},
                AcceptedExtensions = "zip, rar",
                LastScan = DateTime.Now
            };
            await _dbContext.Libraries.AddAsync(library1);

            var library2 = new Library
            {
                Name = "Library 2",
                Folders = new []{new LibraryFolder {Id = Guid.NewGuid(), Path = @"C:\Library 2"}},
                AcceptedExtensions = "cbz",
                LastScan = DateTime.Now
            };
            await _dbContext.Libraries.AddAsync(library2);

            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task Should_return_all_libraries()
        {
            // Act
            var libraries = await _libraryRepository.ListLibrariesAsync();

            // Assert
            libraries.Should().HaveCount(2);
        }
    }

    [TestFixture]
    public class GetLibraryByIdAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private ILibraryRepository _libraryRepository;
        private readonly Guid _libraryId;
        private readonly string _libraryName;
        private readonly string _libraryPath;
        private readonly DateTime _lastScanDateTime;
        private readonly string _acceptedExtensions;

        public GetLibraryByIdAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
            _libraryId = Guid.NewGuid();
            _libraryName = "Library 1";
            _libraryPath = @"C:\Library 1";
            _lastScanDateTime = DateTime.Today;
            _acceptedExtensions = "zip, rar";
        }

        [SetUp]
        public void Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _libraryRepository = new LibraryRepository(_dbContext);
        }

        [Test]
        public async Task Should_return_valid_library()
        {
            // Arrange
            var library = new Library
            {
                Id = _libraryId,
                Name = _libraryName,
                Folders = new []{new LibraryFolder {Id = Guid.NewGuid(), Path = _libraryPath}},
                LastScan = _lastScanDateTime,
                AcceptedExtensions = _acceptedExtensions
            };
            await _dbContext.Libraries.AddAsync(library);

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _libraryRepository.GetLibraryByIdAsync(_libraryId);

            // Assert
            result.Should().NotBeNull();
            if (result != null)
            {
                result.Id.Should().Be(_libraryId);
                result.Name.Should().Be(_libraryName);
                result.Folders.Should().Contain(x => x.Path == _libraryPath);
                result.LastScan.Should().Be(_lastScanDateTime);
                result.AcceptedExtensions.Should().Be(_acceptedExtensions);
            }
        }

        [Test]
        public async Task Should_return_null_when_library_not_found()
        {
            // Act
            var result = await _libraryRepository.GetLibraryByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }
    }

    [TestFixture]
    public class CheckLibraryNameIsUniqueAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private ILibraryRepository _libraryRepository;

        public CheckLibraryNameIsUniqueAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
        }

        [SetUp]
        public async Task Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _libraryRepository = new LibraryRepository(_dbContext);

            var library1 = new Library
            {
                Id = Guid.NewGuid(),
                Name = "Comic Book Library",
                Folders = new []{new LibraryFolder {Id = Guid.NewGuid(), Path = @"C:\Comics"}},
                LastScan = DateTime.Now,
                AcceptedExtensions = "rar, zip"
            };
            await _dbContext.Libraries.AddAsync(library1);

            var library2 = new Library
            {
                Id = Guid.NewGuid(),
                Name = "LibraryRegular",
                Folders = new []{new LibraryFolder {Id = Guid.NewGuid(), Path = @"C:\Library Regular"}},
                LastScan = DateTime.Now,
                AcceptedExtensions = "cbz"
            };
            await _dbContext.Libraries.AddAsync(library2);

            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task Should_return_false_when_name_exists()
        {
            // Act
            var result = await _libraryRepository.CheckLibraryNameIsUniqueAsync("LibraryRegular");

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public async Task Should_return_true_when_name_does_not_exist()
        {
            // Act
            var result = await _libraryRepository.CheckLibraryNameIsUniqueAsync("LibraryDoesNotExist");

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task Should_throw_exception_when_name_is_empty()
        {
            // Act
            Func<Task> action = async () => await _libraryRepository.CheckLibraryNameIsUniqueAsync(string.Empty);
            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Test]
        public async Task Should_throw_exception_when_name_is_whitespace()
        {
            // Act
            Func<Task> action = async () => await _libraryRepository.CheckLibraryNameIsUniqueAsync(" ");
            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }
    }

    [TestFixture]
    public class CheckLibraryPathIsUniqueAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private ILibraryRepository _libraryRepository;

        public CheckLibraryPathIsUniqueAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
        }

        [SetUp]
        public async Task Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _libraryRepository = new LibraryRepository(_dbContext);

            var library1 = new Library
            {
                Id = Guid.NewGuid(),
                Name = "Comic Book Library",
                Folders = new []{new LibraryFolder {Id = Guid.NewGuid(), Path = @"C:\Comics"}},
                LastScan = DateTime.Now,
                AcceptedExtensions = "rar, zip"
            };
            await _dbContext.Libraries.AddAsync(library1);

            var library2 = new Library
            {
                Id = Guid.NewGuid(),
                Name = "LibraryRegular",
                Folders = new []{new LibraryFolder {Id = Guid.NewGuid(), Path = @"C:\Comics"}},
                LastScan = DateTime.Now,
                AcceptedExtensions = "cbz"
            };
            await _dbContext.Libraries.AddAsync(library2);

            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task Should_return_false_when_path_exists()
        {
            // Act
            var result = await _libraryRepository.CheckLibraryFolderPathIsUniqueAsync(@"C:\Library regular");

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public async Task Should_return_true_when_path_does_not_exist()
        {
            // Act
            var result = await _libraryRepository.CheckLibraryFolderPathIsUniqueAsync(@"D:\Library regular");

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public async Task Should_throw_exception_when_path_is_empty()
        {
            // Act
            Func<Task> action = async () => await _libraryRepository.CheckLibraryFolderPathIsUniqueAsync(string.Empty);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Test]
        public async Task Should_throw_exception_when_path_is_whitespace()
        {
            // Act
            Func<Task> action = async () => await _libraryRepository.CheckLibraryFolderPathIsUniqueAsync(" ");

            // Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }
    }

    [TestFixture]
    public class CreateLibraryAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private ILibraryRepository _libraryRepository;

        public CreateLibraryAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
        }

        [SetUp]
        public Task Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _libraryRepository = new LibraryRepository(_dbContext);
            return Task.CompletedTask;
        }

        [Test]
        public async Task Should_create_library()
        {
            // Arrange
            var newLibrary = new Library
            {
                Name = "Comic Book Library",
                Folders = new []{new LibraryFolder {Id = Guid.NewGuid(), Path = @"C:\Comics"}},
                AcceptedExtensions = "7z"
            };

            // Act
            var library = await _libraryRepository.CreateLibraryAsync(newLibrary);
            var result = await _libraryRepository.GetLibraryByIdAsync(library.Id);

            // Assert
            result.Should().NotBeNull();
            if (result != null)
            {
                result.Id.Should().Be(library.Id);
                result.Name.Should().Be(newLibrary.Name);
                result.Folders.Should().Contain(x => x.Path == @"C:\Comics");
                result.AcceptedExtensions.Should().Be(string.Join(",", newLibrary.AcceptedExtensions));
            }
        }

        [Test]
        public async Task Should_throw_exception_when_library_already_exists()
        {
            // Arrange
            var library = new Library
            {
                Id = Guid.NewGuid(),
                Name = "Comic Book Library",
                Folders = new []{new LibraryFolder {Id = Guid.NewGuid(), Path = @"C:\Comics"}},
                LastScan = DateTime.Now,
                AcceptedExtensions = "rar, zip"
            };
            await _dbContext.Libraries.AddAsync(library);
            await _dbContext.SaveChangesAsync();

            var newLibrary = new Library
            {
                Name = library.Name,
                Folders = new []{new LibraryFolder {Id = Guid.NewGuid(), Path = library.Folders.FirstOrDefault()!.Path}},
                AcceptedExtensions = "7z"
            };

            // Act
            Func<Task> result = async () => { await _libraryRepository.CreateLibraryAsync(newLibrary); };

            // Assert
            await result.Should().ThrowAsync<Exception>();
        }

        [Test]
        public async Task Should_throw_exception_when_library_is_null()
        {
            // Act
            Func<Task> result = async () => { await _libraryRepository.CreateLibraryAsync(null); };

            // Assert
            await result.Should().ThrowAsync<Exception>();
        }
    }

    [TestFixture]
    public class UpdateLibraryAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private ILibraryRepository _libraryRepository;
        private readonly Guid _libraryId;

        public UpdateLibraryAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;

            _libraryId = Guid.NewGuid();
        }

        [SetUp]
        public void Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _libraryRepository = new LibraryRepository(_dbContext);
        }

        [Test]
        public async Task Should_update_library()
        {
            // Arrange
            var existentLibrary = new Library
            {
                Id = _libraryId,
                Name = "Comic Book Library",
                Folders = new []{new LibraryFolder {Id = Guid.NewGuid(), Path = @"C:\Comics"}},
                LastScan = DateTime.Now,
                AcceptedExtensions = "rar, zip"
            };
            await _dbContext.Libraries.AddAsync(existentLibrary);

            await _dbContext.SaveChangesAsync();

            var library = new Library
            {
                Name = "My Comic Library",
                Folders = new []{new LibraryFolder {Id = Guid.NewGuid(), Path = @"C:\Comics folder"}},
                AcceptedExtensions = "7z"
            };

            // Act
            await _libraryRepository.UpdateLibraryAsync(library);
            var result = await _libraryRepository.GetLibraryByIdAsync(_libraryId);

            // Assert
            result.Should().NotBeNull();
            if (result != null)
            {
                result.Id.Should().Be(_libraryId);
                result.Name.Should().Be("My Comic Library");
                result.Folders.Should().Contain(x => x.Path == @"C:\Comics folder");
                result.AcceptedExtensions.Should().Be("7z");
            }
        }

        [Test]
        public async Task Should_throw_exception_when_library_is_null()
        {
            // Act
            Func<Task> result = async () => { await _libraryRepository.UpdateLibraryAsync(null); };

            // Assert
            await result.Should().ThrowAsync<Exception>();
        }

        [Test]
        public async Task Should_throw_exception_when_library_does_not_exist()
        {
            // Arrange
            var library = new Library
            {
                Name = "My Comic Library",
                Folders = new []{new LibraryFolder {Id = Guid.NewGuid(), Path = @"C:\Comics folder"}},
                AcceptedExtensions = "7z"
            };

            // Act
            var result = await _libraryRepository.UpdateLibraryAsync(library);

            // Assert
            result.Should().BeFalse();
        }
    }

    [TestFixture]
    public class DeleteLibraryAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private ILibraryRepository _libraryRepository;
        private readonly Guid _libraryId;
        private Library _library;

        public DeleteLibraryAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
            _libraryId = Guid.NewGuid();
        }

        [SetUp]
        public async Task Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _libraryRepository = new LibraryRepository(_dbContext);

            // Create a library
            _library = new Library
            {
                Id = _libraryId,
                Name = "Test Library",
                Folders = new []{new LibraryFolder {Id = Guid.NewGuid(), Path = @"C:\Test library"}},
                LastScan = DateTime.Now,
                AcceptedExtensions = "rar, zip"
            };
            await _dbContext.Libraries.AddAsync(_library);

            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task Should_delete_library()
        {
            // Act
            await _libraryRepository.DeleteLibraryAsync(_library);
            var result = await _libraryRepository.GetLibraryByIdAsync(_libraryId);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public async Task Should_return_false_when_library_does_not_exist()
        {
            // Arrange
            var randomLibrary = new Library
            {
                Id = Guid.NewGuid(),
                Name = "Test Library",
                Folders = new []{new LibraryFolder {Id = Guid.NewGuid(), Path = @"C:\Test Library"}},
                LastScan = DateTime.Now,
                AcceptedExtensions = "rar, zip"
            };
            
            // Act
            var result = await _libraryRepository.DeleteLibraryAsync(randomLibrary);

            // Assert
            result.Should().BeFalse();
        }
    }
}