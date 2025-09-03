using FirstWebApi.Data;
using FirstWebApi.Middleware;
using log4net;
using log4net.Config;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure log4net
var entryAssembly = Assembly.GetEntryAssembly();
if (entryAssembly != null)
{
    var logRepository = LogManager.GetRepository(entryAssembly);
    XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
}
else
{
    // Handle the case where GetEntryAssembly() returns null (e.g., log or throw)
    throw new InvalidOperationException("Entry assembly could not be determined.");
}

// Add services to the container.

builder.Services.AddControllers();

// EF Core with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware
app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<HeaderMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
