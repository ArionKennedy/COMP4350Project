using System.ComponentModel.DataAnnotations;

namespace Palette.Api.Models;

public class CollectionItem
{
    [Key]
    public Guid collection_id { get; set; }

    [Key]
    public Guid media_id { get; set; }

    public DateTime created_at { get; set; }
}