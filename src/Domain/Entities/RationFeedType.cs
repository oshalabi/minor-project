using Domain.EntityTypeConfiguration;

namespace Domain.Entities;

public class RationFeedType
{
    public int Id { get; set; }
    public decimal KgAmount { get; set; }
    public decimal GAmount { get; set; }
    public bool IsEnergyFeed { get; set; } = false;
    public required int FeedTypeId { get; set; }
    public FeedType? FeedType { get; set; }

    public required int RationId { get; set; }
    public Ration? Ration { get; set; }
}