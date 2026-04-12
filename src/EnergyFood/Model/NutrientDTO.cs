namespace EnergyFood.Model;

public class NutrientDTO
{
    public required int Id { get; set; }
    public string? Code { get; set; }
    public decimal Value { get; set; }
    
    public NutrientTypeDTO? NutrientType { get; set; }
    
}