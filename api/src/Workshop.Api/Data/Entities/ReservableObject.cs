namespace Workshop.Api.Data.Entities;

public class ReservableObject
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } = true;
    public bool HasTimeRestrictions { get; set; }
    public TimeOnly? AvailableFrom { get; set; }
    public TimeOnly? AvailableUntil { get; set; }
    public string? DaysOfWeek { get; set; }
    
    // Navigation property
    public Location Location { get; set; } = null!;
}
