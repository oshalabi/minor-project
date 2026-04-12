using Domain.Entities;
using Domain.EntityTypeConfiguration;

namespace BasalRation.Commands;

public class GetAvailableFeedTypes
{
    public required ICollection<CategoryTypeValue> CategoryValues { get; set; }
    public required ICollection<string> FeedTypeKeys { get; set; }
}