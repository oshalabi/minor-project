using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Norms.Models;

namespace Norms.Commands;

public class CompareNorms
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
    public ICollection<NormCompare>? Totals { get; set; }
    public required CowGroupingEnum Group { get; set; }
}
