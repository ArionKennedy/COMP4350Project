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
    public DbSet<Media> Media => Set<Media>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<TvShow> TvShows => Set<TvShow>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Game> Games => Set<Game>();
    public DbSet<UserMediaLog> UserMediaLogs => Set<UserMediaLog>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Collection> Collections => Set<Collection>();
    public DbSet<CollectionItem> CollectionItems => Set<CollectionItem>();

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

        // Media
        modelBuilder.Entity<Media>(entity =>
        {
            entity.ToTable("MEDIA");
        });

        // Movies
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("MOVIES");

            entity.HasIndex(m => m.TmdbMovieId).IsUnique();

            entity.HasOne(m => m.Media)
                .WithOne(m => m.Movie)
                .HasForeignKey<Movie>(m => m.MediaId);
        });

        // TV Shows
        modelBuilder.Entity<TvShow>(entity =>
        {
            entity.ToTable("TV_SHOWS");

            entity.HasIndex(t => t.TmdbTvId).IsUnique();

            entity.HasOne(t => t.Media)
                .WithOne(m => m.TvShow)
                .HasForeignKey<TvShow>(t => t.MediaId);
        });

        // Books
        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("BOOKS");

            entity.HasIndex(b => b.OpenLibraryId).IsUnique();

            entity.HasOne(b => b.Media)
                .WithOne(m => m.Book)
                .HasForeignKey<Book>(b => b.MediaId);
        });

        // Games
        modelBuilder.Entity<Game>(entity =>
        {
            entity.ToTable("GAMES");

            entity.HasIndex(g => g.RawgGameId).IsUnique();

            entity.HasOne(g => g.Media)
                .WithOne(m => m.Game)
                .HasForeignKey<Game>(g => g.MediaId);
        });

        // UserMediaLogs
        modelBuilder.Entity<UserMediaLog>(entity =>
        {
            entity.ToTable("USER_MEDIA_LOGS");

            entity.HasIndex(u => u.LogId).IsUnique();

            entity.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey<UserMediaLog>(u => u.UserId);

            entity.HasOne(u => u.Media)
                .WithMany()
                .HasForeignKey<UserMediaLog>(u => u.MediaId);
        });
    }
}