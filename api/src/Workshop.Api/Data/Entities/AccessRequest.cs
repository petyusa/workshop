namespace Workshop.Api.Data.Entities;

public class AccessRequest
{
    public int Id { get; set; }
    public int ReservableObjectId { get; set; }
    public string RequesterUsername { get; set; } = string.Empty;
    public DateTime RequestedStartTime { get; set; }
    public DateTime RequestedEndTime { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Approved, Denied
    public string? Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? RespondedAt { get; set; }
    public string? ResponseMessage { get; set; }
    
    public ReservableObject ReservableObject { get; set; } = null!;
}
