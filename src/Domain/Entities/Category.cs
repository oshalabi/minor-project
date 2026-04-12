using Domain.EntityTypeConfiguration;

namespace Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public required CategoryTypeValue Value { get; set; }

    public ICollection<FeedType>? FeedTypes { get; set; }
}