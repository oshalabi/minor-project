namespace Norms.Commands;

public class CompareNormsCowCommand
{
    public required int Id { get; set; }
    
    public required int LactationId { get; set; }
    public ICollection<NormCompare>? Totals { get; set; }
}