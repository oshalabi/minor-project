namespace Ration.Commands;

public class UpdateLivestockProperties
{
    public decimal? MilkFat { get; set; }  
    public decimal? MilkProtein { get; set; }
    public int? AvgWeightCow { get; set; }
    public int? TotalCows { get; set; }
    public int? OldCows { get; set; }
    public int? SecondCalves  { get; set; }
    public int? Heiffers  { get; set; }
    public decimal? ProductionLevel { get; set; }
    public decimal? Netto { get; set; }
    public int? CalvingAge  { get; set; }
    public int? AvgLactationDays { get; set; }
}