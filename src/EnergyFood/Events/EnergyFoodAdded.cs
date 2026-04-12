namespace EnergyFood.Events;

public class EnergyFoodAdded
{
    public required int Id { get; set; }
    public required int EnergyFoodId { get; set; }
}