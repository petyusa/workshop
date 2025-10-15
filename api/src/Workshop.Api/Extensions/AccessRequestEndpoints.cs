using Microsoft.EntityFrameworkCore;
using Workshop.Api.Data;
using Workshop.Api.Data.Entities;
using Workshop.Api.Extensions;
using Workshop.Api.Models;

namespace Workshop.Api.Extensions;

public static class AccessRequestEndpoints
{
    public static void MapAccessRequestEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/access-requests");

        group.MapPost("/", CreateAccessRequest)
            .WithName("CreateAccessRequest")
            .WithOpenApi();

        group.MapGet("/my-requests", GetMyRequests)
            .WithName("GetMyRequests")
            .WithOpenApi();

        group.MapGet("/my-owned-objects-requests", GetRequestsForMyOwnedObjects)
            .WithName("GetRequestsForMyOwnedObjects")
            .WithOpenApi();

        group.MapPost("/{id}/respond", RespondToAccessRequest)
            .WithName("RespondToAccessRequest")
            .WithOpenApi();
    }

    private static async Task<IResult> CreateAccessRequest(
        CreateAccessRequestRequest request,
        WorkshopDbContext db,
        HttpContext context)
    {
        var userContext = context.GetCurrentUser();
        if (userContext == null)
        {
            return Results.Unauthorized();
        }

        var obj = await db.ReservableObjects
            .Include(o => o.Location)
            .FirstOrDefaultAsync(o => o.Id == request.ReservableObjectId);

        if (obj == null)
        {
            return Results.BadRequest("Reservable object not found");
        }

        if (string.IsNullOrEmpty(obj.OwnerUsername))
        {
            return Results.BadRequest("This object doesn't have an owner");
        }

        if (obj.OwnerUsername == userContext.Username)
        {
            return Results.BadRequest("You cannot request access to your own object");
        }

        // Validation
        if (request.RequestedStartTime >= request.RequestedEndTime)
        {
            return Results.BadRequest("End time must be after start time");
        }

        if (request.RequestedStartTime < DateTime.UtcNow.AddMinutes(-5))
        {
            return Results.BadRequest("Start time must be in the future");
        }

        // Check for existing pending request
        var existingRequest = await db.AccessRequests
            .Where(ar => ar.ReservableObjectId == request.ReservableObjectId
                      && ar.RequesterUsername == userContext.Username
                      && ar.Status == "Pending")
            .FirstOrDefaultAsync();

        if (existingRequest != null)
        {
            return Results.BadRequest("You already have a pending request for this object");
        }

        // Create access request
        var accessRequest = new AccessRequest
        {
            ReservableObjectId = request.ReservableObjectId,
            RequesterUsername = userContext.Username,
            RequestedStartTime = request.RequestedStartTime,
            RequestedEndTime = request.RequestedEndTime,
            Message = request.Message,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        db.AccessRequests.Add(accessRequest);
        await db.SaveChangesAsync();

        var response = new AccessRequestResponse(
            accessRequest.Id,
            accessRequest.ReservableObjectId,
            obj.Name,
            obj.Type,
            obj.LocationId,
            obj.Location.Name,
            obj.OwnerUsername,
            accessRequest.RequesterUsername,
            accessRequest.RequestedStartTime,
            accessRequest.RequestedEndTime,
            accessRequest.Status,
            accessRequest.Message,
            accessRequest.CreatedAt,
            accessRequest.RespondedAt,
            accessRequest.ResponseMessage
        );

        return Results.Created($"/api/access-requests/{accessRequest.Id}", response);
    }

    private static async Task<IResult> GetMyRequests(
        WorkshopDbContext db,
        HttpContext context,
        string? status = null)
    {
        var userContext = context.GetCurrentUser();
        if (userContext == null)
        {
            return Results.Unauthorized();
        }

        var query = db.AccessRequests
            .Include(ar => ar.ReservableObject)
            .ThenInclude(o => o.Location)
            .Where(ar => ar.RequesterUsername == userContext.Username);

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(ar => ar.Status == status);
        }

        var requests = await query
            .OrderByDescending(ar => ar.CreatedAt)
            .Select(ar => new AccessRequestResponse(
                ar.Id,
                ar.ReservableObjectId,
                ar.ReservableObject.Name,
                ar.ReservableObject.Type,
                ar.ReservableObject.LocationId,
                ar.ReservableObject.Location.Name,
                ar.ReservableObject.OwnerUsername!,
                ar.RequesterUsername,
                ar.RequestedStartTime,
                ar.RequestedEndTime,
                ar.Status,
                ar.Message,
                ar.CreatedAt,
                ar.RespondedAt,
                ar.ResponseMessage
            ))
            .ToListAsync();

        return Results.Ok(requests);
    }

    private static async Task<IResult> GetRequestsForMyOwnedObjects(
        WorkshopDbContext db,
        HttpContext context,
        string? status = null)
    {
        var userContext = context.GetCurrentUser();
        if (userContext == null)
        {
            return Results.Unauthorized();
        }

        var query = db.AccessRequests
            .Include(ar => ar.ReservableObject)
            .ThenInclude(o => o.Location)
            .Where(ar => ar.ReservableObject.OwnerUsername == userContext.Username);

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(ar => ar.Status == status);
        }

        var requests = await query
            .OrderByDescending(ar => ar.CreatedAt)
            .Select(ar => new AccessRequestResponse(
                ar.Id,
                ar.ReservableObjectId,
                ar.ReservableObject.Name,
                ar.ReservableObject.Type,
                ar.ReservableObject.LocationId,
                ar.ReservableObject.Location.Name,
                ar.ReservableObject.OwnerUsername!,
                ar.RequesterUsername,
                ar.RequestedStartTime,
                ar.RequestedEndTime,
                ar.Status,
                ar.Message,
                ar.CreatedAt,
                ar.RespondedAt,
                ar.ResponseMessage
            ))
            .ToListAsync();

        return Results.Ok(requests);
    }

    private static async Task<IResult> RespondToAccessRequest(
        int id,
        RespondToAccessRequestRequest request,
        WorkshopDbContext db,
        HttpContext context)
    {
        var userContext = context.GetCurrentUser();
        if (userContext == null)
        {
            return Results.Unauthorized();
        }

        var accessRequest = await db.AccessRequests
            .Include(ar => ar.ReservableObject)
            .FirstOrDefaultAsync(ar => ar.Id == id);

        if (accessRequest == null)
        {
            return Results.NotFound();
        }

        if (accessRequest.ReservableObject.OwnerUsername != userContext.Username)
        {
            return Results.Forbid();
        }

        if (accessRequest.Status != "Pending")
        {
            return Results.BadRequest("Request has already been responded to");
        }

        accessRequest.Status = request.Approve ? "Approved" : "Denied";
        accessRequest.ResponseMessage = request.ResponseMessage;
        accessRequest.RespondedAt = DateTime.UtcNow;

        // If approved, create a reservation
        if (request.Approve)
        {
            var reservation = new Reservation
            {
                ReservableObjectId = accessRequest.ReservableObjectId,
                Username = accessRequest.RequesterUsername,
                StartTime = accessRequest.RequestedStartTime,
                EndTime = accessRequest.RequestedEndTime,
                Status = "Active",
                CreatedAt = DateTime.UtcNow
            };

            db.Reservations.Add(reservation);
        }

        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}
