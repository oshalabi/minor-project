namespace Ration.Model;

public class CowDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Days { get; set; }
    public decimal Milk { get; set; }
    public decimal Fat { get; set; }
    public decimal Protein { get; set; }
    public decimal RV { get; set; }
    public decimal Total { get; set; }
    public int ParityId { get; set; }
    public string? ParityName { get; set; } 
    public int LactationId { get; set; }
    public string? LactationName { get; set; } 
    public List<AdviceDTO> Advices { get; set; } = new();
}