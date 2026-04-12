namespace Norms.Models;

public class ComparedCowRowDTO
{
    public required int Id { get; set; }
    public ICollection<ComparedColumnDTO> Totals { get; set; }
}