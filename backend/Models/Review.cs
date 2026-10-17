using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Palette.Api.Models;

public class Review
{
    [Key]
    public Guid ReviewId { get; set; }
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    [ForeignKey(nameof(Media))]
    public Guid MediaId { get; set; }
    public int Rating { get; set; }
    public string? Body { get; set; }
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
    public Media Media { get; set; } = null!;
}
