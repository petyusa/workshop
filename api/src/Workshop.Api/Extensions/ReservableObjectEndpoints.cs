using Microsoft.EntityFrameworkCore;
using Workshop.Api.Data;
using Workshop.Api.Models;

namespace Workshop.Api.Extensions;

public static class ReservableObjectEndpoints
{
    public static void MapReservableObjectEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/locations/{locationId}/objects")
            .WithTags("Reservable Objects");

        // Get all reservable objects for a location
        group.MapGet("/", async (int locationId, WorkshopDbContext db, string? type, bool? available) =>
        {
            var query = db.ReservableObjects.Where(o => o.LocationId == locationId);

            // Apply filters
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(o => o.Type == type);
            }

            if (available.HasValue)
            {
                query = query.Where(o => o.IsAvailable == available.Value);
            }

            var objects = await query
                .Select(o => new ReservableObjectResponse(
                    o.Id,
                    o.Name,
                    o.Type,
                    o.IsAvailable,
                    o.HasTimeRestrictions,
                    o.AvailableFrom,
                    o.AvailableUntil,
                    o.DaysOfWeek,
                    o.OwnerUsername
                ))
                .ToListAsync();

            return Results.Ok(new ReservableObjectListResponse(objects.Count, objects));
        })
        .WithName("GetReservableObjects")
        .WithOpenApi()
        .WithDescription("Get all reservable objects for a specific location. Optionally filter by type (Desk, ParkingSpace) or availability.");

        // Get a specific reservable object
        group.MapGet("/{id}", async (int locationId, int id, WorkshopDbContext db) =>
        {
            var obj = await db.ReservableObjects
                .Where(o => o.Id == id && o.LocationId == locationId)
                .Select(o => new ReservableObjectResponse(
                    o.Id,
                    o.Name,
                    o.Type,
                    o.IsAvailable,
                    o.HasTimeRestrictions,
                    o.AvailableFrom,
                    o.AvailableUntil,
                    o.DaysOfWeek,
                    o.OwnerUsername
                ))
                .FirstOrDefaultAsync();

            return obj is not null ? Results.Ok(obj) : Results.NotFound();
        })
        .WithName("GetReservableObjectById")
        .WithOpenApi()
        .WithDescription("Get details of a specific reservable object by ID.");

        // Get all reservable objects (no location filter)
        app.MapGet("/api/objects", async (WorkshopDbContext db, string? type, bool? available) =>
        {
            var query = db.ReservableObjects.AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(o => o.Type == type);
            }

            if (available.HasValue)
            {
                query = query.Where(o => o.IsAvailable == available.Value);
            }

            var objects = await query
                .Select(o => new ReservableObjectResponse(
                    o.Id,
                    o.Name,
                    o.Type,
                    o.IsAvailable,
                    o.HasTimeRestrictions,
                    o.AvailableFrom,
                    o.AvailableUntil,
                    o.DaysOfWeek,
                    o.OwnerUsername
                ))
                .ToListAsync();

            return Results.Ok(new ReservableObjectListResponse(objects.Count, objects));
        })
        .WithName("GetAllReservableObjects")
        .WithOpenApi()
        .WithTags("Reservable Objects")
        .WithDescription("Get all reservable objects across all locations. Optionally filter by type or availability.");
    }
}
