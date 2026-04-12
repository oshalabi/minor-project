namespace EnergyFood.Model;

public class LivestockPropertyDTO
{
    public int Id { get; set; }
    public required decimal BasalRationVEMMaintananceAmount { get; set; }
    public required decimal MilkFat { get; set; }  
    public required decimal MilkProtein { get; set; }
    public required int Gewicht { get; set; }
    
    public required int ParityId { get; set; }
}