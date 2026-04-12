using Domain.EntityTypeConfiguration;
using EnergyFood.Model;

namespace EnergyFood.Commands;

public class ShowMilkProduction
{
    public required decimal EnergyFoodAmount { get; set; }
    public required decimal BasalRationDVPAmount { get; set; }
    public required decimal BasalRationVEMBasicAmount { get; set; } 
}