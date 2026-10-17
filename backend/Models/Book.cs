using System.ComponentModel.DataAnnotations;

namespace Palette.Api.Models;

public class Book
{
    [Key]
    public Guid MediaId { get; set; }
    [Required]
    public string OpenLibraryId { get; set; }
    public string isbn { get; set; }

    public Media Media { get; set; } = null!;
}
