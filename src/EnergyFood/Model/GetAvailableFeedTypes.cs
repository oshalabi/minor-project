using Domain.EntityTypeConfiguration;

namespace EnergyFood.Model;

public class GetAvailableFeedTypes
{
    public required ICollection<CategoryTypeValue> CategoryValues { get; set; }
    public required ICollection<string> FeedTypeKeys { get; set; }
}