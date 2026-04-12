namespace EnergyFood.Model;

public class FeedTypeDTO
{
    public int Id { get; set; }
    public string? Code { get; set; }
    
    public required double DsProcent { get; set; }
    
    public required string Name { get; set; }
    
    public string? CategoryName { get; set; }
    
    public ICollection<NutrientDTO>? Nutrients { get; set; }
}