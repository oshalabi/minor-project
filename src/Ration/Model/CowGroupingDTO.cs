using Newtonsoft.Json;
using Ration.Enums;

namespace Ration.Model;

public class CowGroupingDTO
{
    public required int TotalCows { get; set; }
    public required string Name { get; set; }
    public required double Days { get; set; }
    public required decimal Milk { get; set; }
    public required decimal Fat { get; set; }
    public required decimal Protein { get; set; }
    public required ICollection<AdviceDTO> Advices { get; set; }
    public required decimal RV { get; set; }
    public required decimal Total { get; set; }
    public required CowGroupingEnum Group { get; set; }
}