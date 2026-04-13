using Domain.EntityTypeConfiguration;

namespace Domain.Entities;

public class NutrientType
{
    public int Id { get; set; }
    public required string Code { get; set; }
    public required NutrientTypeValue Value { get; set; }
}