using Domain.EntityTypeConfiguration;

namespace Domain.Entities;

public class LactationPeriod
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required int StartDay { get; set; }
    public int EndDay { get; set; }
    public required LactationPeriodValue Value { get; set; }
    
    public ICollection<Cow>? Cows { get; set; }
}