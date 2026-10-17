using System.ComponentModel.DataAnnotations;

namespace Palette.Api.Models;

public class Collection
{
    [Key]
    public Guid collection_id { get; set; }

    [Key]
    public Guid user_id { get; set; }

    [Required]
    public string? name { get; set; }

    public DateTime created_at { get; set; }
}