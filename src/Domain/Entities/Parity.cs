using Domain.EntityTypeConfiguration;

namespace Domain.Entities;

public class Parity
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required ParityTypeValue ParityTypeValue { get; set; }

    public ICollection<Cow>? Cows { get; set; }
}