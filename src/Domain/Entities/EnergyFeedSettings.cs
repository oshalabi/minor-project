namespace Domain.Entities;

public class EnergyFeedSettings
{
    public int Id { get; set; }
    public required int RationId { get; set; }
    public required int ParityId { get; set; }
    public required int FeedTypeId { get; set; }
    public required int MaxEnergyFeed { get; set; }
    public required int MinEnergyFeed { get; set; }
}