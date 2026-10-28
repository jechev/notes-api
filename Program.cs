using Microsoft.EntityFrameworkCore;
using MyApi.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? "Host=localhost;Database=notes;Username=app;Password=app";

builder.Services.AddDbContext<AppDb>(o => o.UseNpgsql(connectionString));
builder.Services.AddControllers();

var app = builder.Build();

await DatabaseInitializer.WaitForDatabaseAsync(app);

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();