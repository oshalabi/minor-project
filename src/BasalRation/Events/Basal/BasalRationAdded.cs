namespace BasalRation.Events.Basal;

public class BasalRationAdded
{
    public required int Id { get; set; }
    public required int BasalRationId { get; set; }
}