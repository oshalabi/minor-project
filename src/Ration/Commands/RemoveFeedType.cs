namespace Ration.Commands;

public class RemoveFeedType
{
    public required int FeedTypeId { get; set; }
    public required bool IsEnergy { get; set; }
}