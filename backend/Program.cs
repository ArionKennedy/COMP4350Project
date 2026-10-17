using Microsoft.EntityFrameworkCore;
using Palette.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Configure PostgreSQL database
builder.Services.AddDbContext<PaletteDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Development configuration
if (app.Environment.IsDevelopment())
{
    // Automatically apply pending database migrations
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider
            .GetRequiredService<PaletteDbContext>();

        dbContext.Database.Migrate();
    }

    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
