namespace Workshop.Api.Models;

public record AccessRequestResponse(
    int Id,
    int ReservableObjectId,
    string ObjectName,
    string ObjectType,
    int LocationId,
    string LocationName,
    string OwnerUsername,
    string RequesterUsername,
    DateTime RequestedStartTime,
    DateTime RequestedEndTime,
    string Status,
    string? Message,
    DateTime CreatedAt,
    DateTime? RespondedAt,
    string? ResponseMessage
);
