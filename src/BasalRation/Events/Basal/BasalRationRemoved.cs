namespace BasalRation.Events.Basal;

public class BasalRationRemoved
{
    public required int Id { get; set; }
    public required int BasalRationId { get; set; }
}