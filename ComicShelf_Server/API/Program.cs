using API.Extensions;
using Infra.Context;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Models.RepositoryInterfaces;
using Models.ServicesInterfaces;
using Services;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "ComicShelf";

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(SwaggerDocumentationConfig.Config);
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DatabaseContext")));
// Repositories
builder.Services.AddTransient<IComicFileRepository, ComicFileRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ILibraryRepository, LibraryRepository>();
// Services
builder.Services.AddTransient<IComicFileService, ComicFileService>();
builder.Services.AddTransient<IComicInfoService, ComicInfoService>();
builder.Services.AddTransient<IJwtTokenService, JwtTokenService>();
builder.Services.AddTransient<ILibraryService, LibraryService>();
builder.Services.AddTransient<IUserService, UserService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    await context.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();