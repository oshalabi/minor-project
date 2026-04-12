namespace Ration.Commands;

public class UpdateRationCommand
{
    public required int FeedTypeId { get; set; }
    public required bool IsEnergy { get; set; }
    public decimal FeedTypeKg { get; set; }
    public decimal FeedTypeG { get; set; }
}