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
public class FileRepositoryTests
{
    [TestFixture]
    public class SaveAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IComicFileRepository _comicFileRepository;

        public SaveAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
        }

        [SetUp]
        public void Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _comicFileRepository = new ComicFileRepository(_dbContext);
        }
        
        [Test]
        public async Task Should_save_file_to_database()
        {
            // Arrange
            var newFile = new ComicFile
            {
                Name = "TestFile",
                Path = "TestPath",
                Size = 100,
                Extension = "",
                MimeType = ""
            };
            
            // Act
            var file = await _comicFileRepository.SaveFileAsync(newFile);
            var result = await _comicFileRepository.GetFileByIdAsync(file.Id);

            // Assert
            result.Should().NotBeNull();
            if (result != null)
            {
                result.Id.Should().Be(file.Id);
                result.Name.Should().Be(file.Name);
                result.Path.Should().Be(file.Path);
                result.Size.Should().Be(file.Size);
            }
        }
    }

    [TestFixture]
    public class GetFileByNameAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IComicFileRepository _comicFileRepository;
        private readonly Guid _fileId;

        public GetFileByNameAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
            
            _fileId = Guid.NewGuid();
        }

        [SetUp]
        public void Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _comicFileRepository = new ComicFileRepository(_dbContext);
        }

        [Test]
        public async Task Should_return_valid_file()
        {
            // Arrange
            var file = new ComicFile
            {
                Id = _fileId,
                Name = "file1.cbz",
                Path = @"C:\Library 1\file1.cbz",
                Extension = "cbz",
                MimeType = "application/x-cbz",
                Size = 100
            };
            await _dbContext.ComicFiles.AddAsync(file);

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _comicFileRepository.GetFileByNameAsync(file.Name);

            // Assert
            result.Should().NotBeNull();
            if (result != null)
            {
                result.Id.Should().Be(_fileId);
                result.Name.Should().Be(file.Name);
                result.Path.Should().Be(file.Path);
                result.Extension.Should().Be(file.Extension);
                result.MimeType.Should().Be(file.MimeType);
                result.Size.Should().Be(file.Size);
            }
        }

        [Test]
        public async Task Should_return_null_when_file_not_found()
        {
            // Act
            var result = await _comicFileRepository.GetFileByNameAsync("Testing name");

            // Assert
            result.Should().BeNull();
        }
    }

    [TestFixture]
    public class GetFileByIdAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IComicFileRepository _comicFileRepository;
        private readonly Guid _fileId;

        public GetFileByIdAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
            
            _fileId = Guid.NewGuid();
        }

        [SetUp]
        public void Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _comicFileRepository = new ComicFileRepository(_dbContext);
        }

        [Test]
        public async Task Should_return_valid_file()
        {
            // Arrange
            var file = new ComicFile
            {
                Id = _fileId,
                Name = "file1.cbz",
                Path = @"C:\Library 1\file1.cbz",
                Extension = "cbz",
                MimeType = "application/x-cbz",
                Size = 100
            };
            await _dbContext.ComicFiles.AddAsync(file);

            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _comicFileRepository.GetFileByIdAsync(_fileId);

            // Assert
            result.Should().NotBeNull();
            if (result != null)
            {
                result.Id.Should().Be(_fileId);
                result.Name.Should().Be(file.Name);
                result.Path.Should().Be(file.Path);
                result.Extension.Should().Be(file.Extension);
                result.MimeType.Should().Be(file.MimeType);
                result.Size.Should().Be(file.Size);
            }
        }

        [Test]
        public async Task Should_return_null_when_file_not_found()
        {
            // Act
            var result = await _comicFileRepository.GetFileByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }
    }

    [TestFixture]
    public class ReturnFilesAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IComicFileRepository _comicFileRepository;

        public ReturnFilesAsync()
        {
            _dbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase($"TestsDb_{DateTime.Now.ToFileTimeUtc()}")
                .Options;
        }

        [SetUp]
        public async Task Setup()
        {
            _dbContext = new DatabaseContext(_dbContextOptions);
            _comicFileRepository = new ComicFileRepository(_dbContext);

            // File 1
            var file1 = new ComicFile
            {
                Id = Guid.NewGuid(),
                Name = "",
                Extension = "cbz",
                Path = "",
                Size = 100,
                CreatedAt = DateTime.Now,
                MimeType = "",
                UpdatedAt = DateTime.Now
            };
            await _dbContext.ComicFiles.AddAsync(file1);

            // File 2
            var file2 = new ComicFile
            {
                Id = Guid.NewGuid(),
                Name = "",
                Extension = "zip",
                Path = "",
                Size = 100,
                CreatedAt = DateTime.Now,
                MimeType = "",
                UpdatedAt = DateTime.Now
            };
            await _dbContext.ComicFiles.AddAsync(file2);

            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task Should_return_all_files()
        {
            // Act
            var users = await _comicFileRepository.ReturnFilesAsync();

            // Assert
            users.Should().HaveCount(2);
        }
    }
}