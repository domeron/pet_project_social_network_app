using Microsoft.EntityFrameworkCore;
using Serilog;
using SocialNetworkAppAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging
    .ClearProviders()
    .AddSimpleConsole()
    .AddDebug();

builder.Host.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(ctx.Configuration);
    lc.WriteTo.File("Logs/log.txt",
        outputTemplate:
            "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:1j} {NewLine} {Exception}",
        rollingInterval: RollingInterval.Day);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SqlServerConnection"));
});

var app = builder.Build();

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
