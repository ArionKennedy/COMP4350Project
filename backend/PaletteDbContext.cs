using System.Dynamic;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Palette.Api.Models;

namespace Palette.Api.Data;

public class PaletteDbContext : DbContext
{
    public PaletteDbContext(DbContextOptions<PaletteDbContext> options)
        : base(options)
    {
    }

    // entity tables
    public DbSet<User> Users => Set<User>(); 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Users
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("USERS");
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Auth0Id).IsUnique();
        });
    }
}