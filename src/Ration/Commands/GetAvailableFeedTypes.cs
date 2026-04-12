namespace Ration.Commands;

public class GetAvailableFeedTypes
{
    public required ICollection<int> CategoryIds { get; set; }
}