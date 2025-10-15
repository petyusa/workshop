namespace Workshop.Api.Models;

public record CreateAccessRequestRequest(
    int ReservableObjectId,
    DateTime RequestedStartTime,
    DateTime RequestedEndTime,
    string? Message
);

public record RespondToAccessRequestRequest(
    bool Approve,
    string? ResponseMessage
);
