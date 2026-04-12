namespace Ration.Model;

public class EnergyFeedSettingsDto
{
    public int RationId { get; set; }
    public int ParityId { get; set; }
    public int FeedTypeId { get; set; }
    public string FeedTypeName { get; set; }
    public int MinEnergyFeed { get; set; }
    public int MaxEnergyFeed { get; set; }
 }