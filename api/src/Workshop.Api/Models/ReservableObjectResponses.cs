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
    string? OwnerUsername
);

public record ReservableObjectListResponse(
    int TotalCount,
    List<ReservableObjectResponse> Objects
);
