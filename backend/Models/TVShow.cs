using System.ComponentModel.DataAnnotations;

namespace Palette.Api.Models;

public class TvShow
{
    [Key]
    public Guid MediaId { get; set; }
    [Required]
    public int TmdbTvId { get; set; }
    public int? NumberOfSeasons { get; set; }

    public Media Media { get; set; } = null!;
}
