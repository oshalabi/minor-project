namespace Ration.Model;

public class FeedTypeOverviewDTO
{
    public required int Id { get; set; }
    public required string Name { get; set; }

    public decimal? RationTotalKg { get; set; }

    public decimal? RationTotalKgDs { get; set; }

    public int? DsProcentWeightedSum { get; set; }

    public List<KeyValuePair<string, int>>? AverageTotalFeedTypes { get; set; }
    public ICollection<RationFeedTypeDTO>? FeedTypes { get; set; }
}