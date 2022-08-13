using Microsoft.EntityFrameworkCore;
using Models.Domain;

namespace Infra.Context;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        :base(options)
    { }
    
    public DbSet<ComicFile> ComicFiles { get; set; }
    public DbSet<Library> Libraries { get; set; }
    public DbSet<User> Users { get; set; }
}