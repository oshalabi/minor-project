namespace Domain.Entities;

public class Nutrient
{
    public int Id { get; set; }
    public required decimal Value { get; set; }
    public required int NutrientTypeId { get; set; }
    public NutrientType? NutrientType { get; set; } 
    
    // Many-to-many relationship with FeedType
    public ICollection<FeedType>? FeedTypes { get; set; }
}