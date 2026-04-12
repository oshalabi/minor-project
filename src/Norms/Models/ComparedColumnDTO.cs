namespace Norms.Models;

public class ComparedColumnDTO
{
    public required string Field { get; set; }
    public required decimal Value { get; set; }
    public required int Warning { get; set; }
    public string? Norm { get; set; }
}