using System;
using System.Threading.Tasks;
using API.Context;
using API.Interfaces;
using API.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Models;
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
        private IFileRepository _fileRepository;

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
            _fileRepository = new FileRepository(_dbContext);
        }
        
        [Test]
        public async Task Should_save_file_to_database()
        {
            // Arrange
            var newFile = new File
            {
                Name = "TestFile",
                Path = "TestPath",
                Size = 100,
                Extension = "",
                MimeType = ""
            };
            
            // Act
            var file = await _fileRepository.SaveAsync(newFile);
            var result = await _fileRepository.GetFileByIdAsync(file.Id);

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
        
    }

    [TestFixture]
    public class GetFileByIdAsync
    {
        
    }

    [TestFixture]
    public class ReturnFilesAsync
    {
        private readonly DbContextOptions<DatabaseContext> _dbContextOptions;
        private DatabaseContext _dbContext;
        private IFileRepository _fileRepository;

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
            _fileRepository = new FileRepository(_dbContext);

            // File 1
            var file1 = new File
            {
                Id = Guid.NewGuid(),
                Name = "",
                Extension = "cbz",
                Path = "",
                Size = 100,
                AddedAt = DateTime.Now,
                MimeType = "",
                LastModifiedDate = DateTime.Now
            };
            await _dbContext.Files.AddAsync(file1);

            // File 2
            var file2 = new File
            {
                Id = Guid.NewGuid(),
                Name = "",
                Extension = "zip",
                Path = "",
                Size = 100,
                AddedAt = DateTime.Now,
                MimeType = "",
                LastModifiedDate = DateTime.Now
            };
            await _dbContext.Files.AddAsync(file2);

            await _dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task Should_return_all_files()
        {
            // Act
            var users = await _fileRepository.ReturnFilesAsync();

            // Assert
            users.Should().HaveCount(2);
        }
    }
}