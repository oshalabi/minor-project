using Domain.EntityTypeConfiguration;

namespace Domain.Entities;

public class Norm
{
    public int Id { get; set; }
    public required RationTypeValue RationType { get; set; }
    public required string Name { get; set; }
    public Decimal? MinValue { get; set; }
    public Decimal? MaxValue { get; set; }
    public string? Remark { get; set; }
    
    public int? NutrientTypeId { get; set; }
    public NutrientType? NutrientType { get; set; }
    
    public int? LactationPeriodId { get; set; }
    public LactationPeriod? LactationPeriod { get; set; }
}