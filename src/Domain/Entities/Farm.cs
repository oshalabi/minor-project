namespace Domain.Entities;

public class Farm
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Location { get; set; }

    public int? LivestockFeedAdvisorId { get; set; }
    public LivestockFeedAdvisor? LivestockFeedAdvisor { get; set; }

    public ICollection<Cow>? Cows { get; set; }
}