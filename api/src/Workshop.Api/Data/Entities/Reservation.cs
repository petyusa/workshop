namespace Workshop.Api.Data.Entities;

public class Reservation
{
    public int Id { get; set; }
    public int ReservableObjectId { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; }
    
    public ReservableObject ReservableObject { get; set; } = null!;
}
