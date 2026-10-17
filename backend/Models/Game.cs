using System.ComponentModel.DataAnnotations;

namespace Palette.Api.Models;

public class Game
{
    [Key]
    public Guid MediaId { get; set; }
    [Required]
    public int RawgGameId { get; set; }

    public Media Media { get; set; } = null!;
}
