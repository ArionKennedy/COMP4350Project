using Microsoft.EntityFrameworkCore;

namespace Palette.Api.Data;

public class PaletteDbContext : DbContext
{
    public PaletteDbContext(DbContextOptions<PaletteDbContext> options)
        : base(options)
    {
    }
}