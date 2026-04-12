namespace Domain.Entities;

public class Cow
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required int Days  { get; set; }
    public required decimal Milk { get; set; }
    public required decimal Fat { get; set; }
    public required decimal Protein { get; set; }
    public required List<Advice> Advices { get; set; }
    public required decimal RV { get; set; }
    public required decimal Total { get; set; }
    
    public required int ParityId { get; set; }
    public Parity Parity { get; set; }
    
    public required int LactationPeriodId { get; set; }
    public LactationPeriod LactationPeriod { get; set; }
    
    public int? FarmId { get; set; }
    public Farm Farm { get; set; } = null!;
}