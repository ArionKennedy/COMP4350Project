using System.ComponentModel.DataAnnotations.Schema;

namespace Palette.Api.Models;

public class CollectionItem
{
    [ForeignKey(nameof(Collection))]
    public Guid CollectionId { get; set; }
    [ForeignKey(nameof(Media))]
    public Guid MediaId { get; set; }
    public DateTime AddedAt { get; set; }

    public Collection Collection { get; set; } = null!;
    public Media Media { get; set; } = null!;
}
