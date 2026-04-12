namespace Domain.Entities;

public class Farmer
{
    public required int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    

    public int? FarmId { get; set; }
    public Farm? Farm { get; set; }
}