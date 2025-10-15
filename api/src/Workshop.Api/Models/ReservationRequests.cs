namespace Workshop.Api.Models;

public record CreateReservationRequest(
    int ReservableObjectId,
    DateTime StartTime,
    DateTime EndTime
);
