using API.Context;
using API.Interfaces;
using API.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Models;

var builder = WebApplication.CreateBuilder(args);
const string serviceName = "ComicShelf";

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("IsAdmin"));
    options.AddPolicy("OdpsAccessOnly", policy => policy.RequireClaim("CanAccessOdps"));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = serviceName, Version = "v1" });

    var filePath = Path.Combine(AppContext.BaseDirectory, "Api.xml");
    c.IncludeXmlComments(filePath);
});
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DatabaseContext")));
builder.Services.Configure<LibrariesConfig>(builder.Configuration.GetSection("LibrariesConfig"));
builder.Services.AddTransient<IFileRepository, FileRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ILibraryRepository, LibraryRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    context.Database.Migrate();
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