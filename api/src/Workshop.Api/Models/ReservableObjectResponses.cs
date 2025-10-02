namespace Workshop.Api.Models;

public record ReservableObjectResponse(
    int Id,
    string Name,
    string Type,
    bool IsAvailable,
    bool HasTimeRestrictions,
    TimeOnly? AvailableFrom,
    TimeOnly? AvailableUntil,
    string? DaysOfWeek,
    int? PositionX,
    int? PositionY
);

public record ReservableObjectListResponse(
    int TotalCount,
    List<ReservableObjectResponse> Objects
);
