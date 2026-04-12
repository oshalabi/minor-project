namespace Domain.Entities;

public class LivestockFeedAdvisor
{
    public required int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public ICollection<Farm> Farms { get; set; }
}