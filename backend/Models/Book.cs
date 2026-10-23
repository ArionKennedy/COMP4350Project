using System.ComponentModel.DataAnnotations;

namespace Palette.Api.Models;

public class Book
{
    [Key]
    public Guid MediaId { get; set; }
    [Required]
    public string OpenLibraryId { get; set; } = string.Empty;
    public string? ISBN { get; set; }

    public Media Media { get; set; } = null!;
}
