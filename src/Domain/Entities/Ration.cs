namespace Domain.Entities;

public class Ration
{
    public int Id { get; set; }
    public required string Name { get; set; }
    
    public required int LivestockPropertyId { get; set; }
    public LivestockProperties LivestockProperties { get; set; } = null!;
    
    public ICollection<RationFeedType>? FeedTypes { get; set; }
}