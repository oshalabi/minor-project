using Domain.EntityTypeConfiguration;

namespace EnergyFood.Commands;

public class GetAvailableFeedTypes
{
    public required ICollection<CategoryTypeValue> CategoryValues { get; set; }
    public required ICollection<string> FeedTypeKeys { get; set; }
}