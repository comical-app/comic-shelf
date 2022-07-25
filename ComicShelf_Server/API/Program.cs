using API.Context;
using API.Interfaces;
using API.Repositories;
using Microsoft.EntityFrameworkCore;
using Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DatabaseContext")));
builder.Services.Configure<LibrariesConfig>(builder.Configuration.GetSection("LibrariesConfig"));
builder.Services.AddTransient<IFileRepository, FileRepository>();

// using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
// {
//     var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//     context.Database.Migrate();
// }

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    context.Database.Migrate();
    context.Database.up
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

app.Run();