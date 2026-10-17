using System.ComponentModel.DataAnnotations;

namespace Palette.Api.Models;

public class Review
{
    [Key]
    public Guid ReviewId { get; set; }

    [Key]
    public Guid UserId { get; set; }

    [Key]
    public Guid MediaId { get; set; }

    public int rating { get; set; }

    public string body { get; set; }

    public DateTime created_at { get; set; }
}