using System.ComponentModel.DataAnnotations;

namespace Palette.Api.Models;

public class UserMediaLog
{
    [Key]
    public Guid LogId { get; set; }

    [Key]
    public Guid UserId { get; set; }

    [Key]
    public Guid MediaId { get; set; }

    public string? Status { get; set; }

    public DateOnly LoggedDate { get; set; }

    public Media Media { get; set; } = null!;
    public User User { get; set; } = null!;
}
