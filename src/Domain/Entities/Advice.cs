namespace Domain.Entities;

public class Advice
{
    public int Id { get; set; }
    public required decimal Value { get; set; }
    public required int CowId { get; set; }
    public Cow Cow { get; set; } = null!;
    
}