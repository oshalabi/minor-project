namespace Ration.Model;

public class RationFeedTypeDTO
{

    public decimal KgAmount { get; set; }
    public decimal GAmount { get; set; }
    public FeedTypeDTO? FeedType { get; set; }
}