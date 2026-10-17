using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Palette.Api.Models;

public class Collection
{
    [Key]
    public Guid CollectionId { get; set; }
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public User User { get; set; } = null!;
}
