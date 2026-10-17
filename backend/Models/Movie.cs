
using System.ComponentModel.DataAnnotations;

namespace Palette.Api.Models;

public class Movie
{
    [Key]
    public Guid MediaId { get; set; }
    [Required]
    public int TmdbMovieId { get; set; }
    public int? RuntimeMinutes { get; set; }

    public Media Media { get; set; } = null!;
}
