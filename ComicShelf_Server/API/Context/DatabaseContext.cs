using Microsoft.EntityFrameworkCore;
using File = Models.File;

namespace API.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        :base(options)
    { }
    
    // public DbSet<ComicFile> ComicFiles { get; set; }
    public DbSet<File?> Files { get; set; }
    // public DbSet<Library> Libraries { get; set; }
}