namespace Workshop.Api.Models;

public record ReservationResponse(
    int Id,
    int ReservableObjectId,
    string ObjectName,
    string ObjectType,
    int LocationId,
    string LocationName,
    string Username,
    DateTime StartTime,
    DateTime EndTime,
    string Status,
    DateTime CreatedAt
);

public record ReservationConflictResponse(
    bool HasConflict,
    string? Message,
    List<ConflictingReservation>? Conflicts
);

public record ConflictingReservation(
    int Id,
    DateTime StartTime,
    DateTime EndTime
);
