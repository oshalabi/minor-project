namespace Domain.Entities;

public class FeedType
{
    public int Id { get; set; }

    public string? Code { get; set; }
    public required string Name { get; set; }
    public required double DsProcent { get; set; }
    public required int CategoryId { get; set; }
    public Category? Category { get; set; }
    public ICollection<Nutrient>? Nutrients { get; set; }

    public ICollection<RationFeedType>? RationFeedTypes { get; set; }
}