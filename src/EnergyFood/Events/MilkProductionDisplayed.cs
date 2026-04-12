using Domain.EntityTypeConfiguration;

namespace EnergyFood.Events;

public class MilkProductionDisplayed
{
    public required int Id { get; set; }
    
    public LactationPeriodValue lactationPeriodId { get; set; }
    
    public required int basalRationDVPAmount { get; set; }
    
    public required float basalRationVEMBasicAmount { get; set; }
    
    public required float basalRationVEMMaintananceAmount { get; set; }
    
    public required float milkFat { get; set; }
    
    public required float milkProtein { get; set; }
    
    public required int gewicht { get; set; }
}