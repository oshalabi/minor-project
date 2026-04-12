namespace EnergyFood.Events;

public class EnergyFoodRemoved
{
    public required int Id { get; set; }
    public required int EnergyFoodId { get; set; }
}