using Microsoft.EntityFrameworkCore;
using Workshop.Api.Data;
using Workshop.Api.Data.Entities;
using Workshop.Api.Models;

namespace Workshop.Api.Extensions;

public static class ReservationEndpoints
{
    public static void MapReservationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reservations");

        group.MapPost("/", CreateReservation)
            .WithName("CreateReservation")
            .WithOpenApi();

        group.MapPost("/check-availability", CheckAvailability)
            .WithName("CheckAvailability")
            .WithOpenApi();

        group.MapGet("/my-reservations", GetMyReservations)
            .WithName("GetMyReservations")
            .WithOpenApi();

        group.MapDelete("/{id}", CancelReservation)
            .WithName("CancelReservation")
            .WithOpenApi();
    }

    private static async Task<IResult> CreateReservation(
        CreateReservationRequest request,
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

        // Validation
        if (request.StartTime >= request.EndTime)
        {
            return Results.BadRequest("End time must be after start time");
        }

        if (request.StartTime < DateTime.UtcNow.AddMinutes(-5))
        {
            return Results.BadRequest("Start time must be in the future");
        }

        var duration = request.EndTime - request.StartTime;
        var maxDuration = obj.Type == "Desk" ? TimeSpan.FromHours(8) : TimeSpan.FromHours(12);
        if (duration > maxDuration)
        {
            return Results.BadRequest($"Duration exceeds maximum allowed ({maxDuration.TotalHours} hours for {obj.Type})");
        }

        // Check time restrictions
        if (obj.HasTimeRestrictions)
        {
            var validationError = ValidateTimeRestrictions(obj, request.StartTime, request.EndTime);
            if (validationError != null)
            {
                return Results.BadRequest(validationError);
            }
        }

        // Check for conflicts
        var conflicts = await db.Reservations
            .Where(r => r.ReservableObjectId == request.ReservableObjectId 
                     && r.Status == "Active"
                     && !(request.EndTime <= r.StartTime || request.StartTime >= r.EndTime))
            .ToListAsync();

        if (conflicts.Any())
        {
            return Results.BadRequest("Time slot conflicts with existing reservation");
        }

        // Create reservation
        var reservation = new Reservation
        {
            ReservableObjectId = request.ReservableObjectId,
            Username = userContext.Username,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Status = "Active",
            CreatedAt = DateTime.UtcNow
        };

        db.Reservations.Add(reservation);
        await db.SaveChangesAsync();

        var response = new ReservationResponse(
            reservation.Id,
            reservation.ReservableObjectId,
            obj.Name,
            obj.Type,
            obj.LocationId,
            obj.Location.Name,
            reservation.Username,
            reservation.StartTime,
            reservation.EndTime,
            reservation.Status,
            reservation.CreatedAt
        );

        return Results.Created($"/api/reservations/{reservation.Id}", response);
    }

    private static async Task<IResult> CheckAvailability(
        CreateReservationRequest request,
        WorkshopDbContext db,
        HttpContext context)
    {
        var userContext = context.GetCurrentUser();
        if (userContext == null)
        {
            return Results.Unauthorized();
        }

        var obj = await db.ReservableObjects.FindAsync(request.ReservableObjectId);
        if (obj == null)
        {
            return Results.Ok(new ReservationConflictResponse(true, "Reservable object not found", null));
        }

        // Validation
        if (request.StartTime >= request.EndTime)
        {
            return Results.Ok(new ReservationConflictResponse(true, "End time must be after start time", null));
        }

        if (request.StartTime < DateTime.UtcNow.AddMinutes(-5))
        {
            return Results.Ok(new ReservationConflictResponse(true, "Start time must be in the future", null));
        }

        var duration = request.EndTime - request.StartTime;
        var maxDuration = obj.Type == "Desk" ? TimeSpan.FromHours(8) : TimeSpan.FromHours(12);
        if (duration > maxDuration)
        {
            return Results.Ok(new ReservationConflictResponse(true, $"Duration exceeds maximum allowed ({maxDuration.TotalHours} hours for {obj.Type})", null));
        }

        // Check time restrictions
        if (obj.HasTimeRestrictions)
        {
            var validationError = ValidateTimeRestrictions(obj, request.StartTime, request.EndTime);
            if (validationError != null)
            {
                return Results.Ok(new ReservationConflictResponse(true, validationError, null));
            }
        }

        // Check for conflicts
        var conflicts = await db.Reservations
            .Where(r => r.ReservableObjectId == request.ReservableObjectId 
                     && r.Status == "Active"
                     && !(request.EndTime <= r.StartTime || request.StartTime >= r.EndTime))
            .Select(r => new ConflictingReservation(r.Id, r.StartTime, r.EndTime))
            .ToListAsync();

        if (conflicts.Any())
        {
            return Results.Ok(new ReservationConflictResponse(true, "Time slot conflicts with existing reservation", conflicts));
        }

        return Results.Ok(new ReservationConflictResponse(false, null, null));
    }

    private static async Task<IResult> GetMyReservations(
        WorkshopDbContext db,
        HttpContext context,
        string? status = null,
        DateTime? startDate = null,
        DateTime? endDate = null)
    {
        var userContext = context.GetCurrentUser();
        if (userContext == null)
        {
            return Results.Unauthorized();
        }

        var query = db.Reservations
            .Include(r => r.ReservableObject)
            .ThenInclude(o => o.Location)
            .Where(r => r.Username == userContext.Username);

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(r => r.Status == status);
        }

        if (startDate.HasValue)
        {
            query = query.Where(r => r.StartTime >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(r => r.EndTime <= endDate.Value);
        }

        var reservations = await query
            .OrderByDescending(r => r.StartTime)
            .Select(r => new ReservationResponse(
                r.Id,
                r.ReservableObjectId,
                r.ReservableObject.Name,
                r.ReservableObject.Type,
                r.ReservableObject.LocationId,
                r.ReservableObject.Location.Name,
                r.Username,
                r.StartTime,
                r.EndTime,
                r.Status,
                r.CreatedAt
            ))
            .ToListAsync();

        return Results.Ok(reservations);
    }

    private static async Task<IResult> CancelReservation(
        int id,
        WorkshopDbContext db,
        HttpContext context)
    {
        var userContext = context.GetCurrentUser();
        if (userContext == null)
        {
            return Results.Unauthorized();
        }

        var reservation = await db.Reservations.FindAsync(id);
        if (reservation == null)
        {
            return Results.NotFound();
        }

        if (reservation.Username != userContext.Username)
        {
            return Results.Forbid();
        }

        if (reservation.EndTime < DateTime.UtcNow)
        {
            return Results.BadRequest("Cannot cancel past reservations");
        }

        reservation.Status = "Cancelled";
        await db.SaveChangesAsync();

        return Results.NoContent();
    }

    private static string? ValidateTimeRestrictions(ReservableObject obj, DateTime startTime, DateTime endTime)
    {
        if (!obj.HasTimeRestrictions) return null;

        var startTimeOnly = TimeOnly.FromDateTime(startTime);
        var endTimeOnly = TimeOnly.FromDateTime(endTime);

        if (obj.AvailableFrom.HasValue && startTimeOnly < obj.AvailableFrom.Value)
        {
            return $"Start time is before available hours (available from {obj.AvailableFrom.Value:HH:mm})";
        }

        if (obj.AvailableUntil.HasValue && endTimeOnly > obj.AvailableUntil.Value)
        {
            return $"End time is after available hours (available until {obj.AvailableUntil.Value:HH:mm})";
        }

        if (!string.IsNullOrEmpty(obj.DaysOfWeek))
        {
            var allowedDays = obj.DaysOfWeek.Split(',').Select(d => d.Trim()).ToList();
            var startDay = startTime.ToString("ddd");
            var endDay = endTime.ToString("ddd");

            if (!allowedDays.Contains(startDay) || !allowedDays.Contains(endDay))
            {
                return $"Reservation days not allowed (available on: {obj.DaysOfWeek})";
            }
        }

        return null;
    }
}
