using Microsoft.EntityFrameworkCore;
using Workshop.Api.Data.Entities;

namespace Workshop.Api.Data;

public class WorkshopDbContext : DbContext
{
    public WorkshopDbContext(DbContextOptions<WorkshopDbContext> options) : base(options)
    {
    }

    public DbSet<Location> Locations => Set<Location>();
    public DbSet<ReservableObject> ReservableObjects => Set<ReservableObject>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<AccessRequest> AccessRequests => Set<AccessRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Location entity
        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.IsActive).IsRequired();
        });

        // Configure ReservableObject entity
        modelBuilder.Entity<ReservableObject>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Type).IsRequired().HasMaxLength(50);
            entity.Property(e => e.IsAvailable).IsRequired();
            entity.Property(e => e.HasTimeRestrictions).IsRequired();
            entity.Property(e => e.DaysOfWeek).HasMaxLength(100);
            entity.Property(e => e.OwnerUsername).HasMaxLength(100);

            entity.HasOne(e => e.Location)
                  .WithMany()
                  .HasForeignKey(e => e.LocationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Reservation entity
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.StartTime).IsRequired();
            entity.Property(e => e.EndTime).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasOne(e => e.ReservableObject)
                  .WithMany()
                  .HasForeignKey(e => e.ReservableObjectId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.ReservableObjectId);
            entity.HasIndex(e => e.Username);
            entity.HasIndex(e => e.StartTime);
            entity.HasIndex(e => e.Status);
        });

        // Configure AccessRequest entity
        modelBuilder.Entity<AccessRequest>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.RequesterUsername).IsRequired().HasMaxLength(100);
            entity.Property(e => e.RequestedStartTime).IsRequired();
            entity.Property(e => e.RequestedEndTime).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.ResponseMessage).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).IsRequired();

            entity.HasOne(e => e.ReservableObject)
                  .WithMany()
                  .HasForeignKey(e => e.ReservableObjectId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.ReservableObjectId);
            entity.HasIndex(e => e.RequesterUsername);
            entity.HasIndex(e => e.Status);
        });

        // Seed initial locations
        modelBuilder.Entity<Location>().HasData(
            new Location 
            { 
                Id = 1, 
                Name = "Downtown Location", 
                Address = "123 Main Street, Downtown",
                IsActive = true 
            },
            new Location 
            { 
                Id = 2, 
                Name = "North Branch", 
                Address = "456 North Avenue",
                IsActive = true 
            },
            new Location 
            { 
                Id = 3, 
                Name = "East Side Location", 
                Address = "789 East Boulevard",
                IsActive = true 
            }
        );

        // Seed reservable objects
        modelBuilder.Entity<ReservableObject>().HasData(
            // Downtown Location (Id = 1) - Desks
            new ReservableObject { Id = 1, LocationId = 1, Name = "Desk-A1", Type = "Desk", IsAvailable = true, HasTimeRestrictions = false, OwnerUsername = "john.doe" },
            new ReservableObject { Id = 2, LocationId = 1, Name = "Desk-A2", Type = "Desk", IsAvailable = true, HasTimeRestrictions = false, OwnerUsername = "jane.smith" },
            new ReservableObject { Id = 3, LocationId = 1, Name = "Desk-A3", Type = "Desk", IsAvailable = false, HasTimeRestrictions = false },
            new ReservableObject { Id = 4, LocationId = 1, Name = "Desk-B1", Type = "Desk", IsAvailable = true, HasTimeRestrictions = true, AvailableFrom = new TimeOnly(8, 0), AvailableUntil = new TimeOnly(18, 0), DaysOfWeek = "Mon,Tue,Wed,Thu,Fri" },
            new ReservableObject { Id = 5, LocationId = 1, Name = "Desk-B2", Type = "Desk", IsAvailable = true, HasTimeRestrictions = true, AvailableFrom = new TimeOnly(8, 0), AvailableUntil = new TimeOnly(18, 0), DaysOfWeek = "Mon,Tue,Wed,Thu,Fri" },
            new ReservableObject { Id = 6, LocationId = 1, Name = "Desk-C1", Type = "Desk", IsAvailable = true, HasTimeRestrictions = false },
            new ReservableObject { Id = 7, LocationId = 1, Name = "Desk-C2", Type = "Desk", IsAvailable = true, HasTimeRestrictions = false },
            
            // Downtown Location - Parking Spaces
            new ReservableObject { Id = 8, LocationId = 1, Name = "Parking-P1", Type = "ParkingSpace", IsAvailable = true, HasTimeRestrictions = false, OwnerUsername = "bob.wilson" },
            new ReservableObject { Id = 9, LocationId = 1, Name = "Parking-P2", Type = "ParkingSpace", IsAvailable = false, HasTimeRestrictions = false },
            new ReservableObject { Id = 10, LocationId = 1, Name = "Parking-P3", Type = "ParkingSpace", IsAvailable = true, HasTimeRestrictions = true, AvailableFrom = new TimeOnly(6, 0), AvailableUntil = new TimeOnly(22, 0), DaysOfWeek = "Mon,Tue,Wed,Thu,Fri,Sat,Sun" },
            new ReservableObject { Id = 11, LocationId = 1, Name = "Parking-P4", Type = "ParkingSpace", IsAvailable = true, HasTimeRestrictions = false },

            // North Branch (Id = 2) - Desks
            new ReservableObject { Id = 12, LocationId = 2, Name = "Desk-N1", Type = "Desk", IsAvailable = true, HasTimeRestrictions = false },
            new ReservableObject { Id = 13, LocationId = 2, Name = "Desk-N2", Type = "Desk", IsAvailable = true, HasTimeRestrictions = false },
            new ReservableObject { Id = 14, LocationId = 2, Name = "Desk-N3", Type = "Desk", IsAvailable = true, HasTimeRestrictions = false },
            new ReservableObject { Id = 15, LocationId = 2, Name = "Desk-N4", Type = "Desk", IsAvailable = false, HasTimeRestrictions = false },
            new ReservableObject { Id = 16, LocationId = 2, Name = "Desk-N5", Type = "Desk", IsAvailable = true, HasTimeRestrictions = true, AvailableFrom = new TimeOnly(9, 0), AvailableUntil = new TimeOnly(17, 0), DaysOfWeek = "Mon,Tue,Wed,Thu,Fri" },
            
            // North Branch - Parking Spaces
            new ReservableObject { Id = 17, LocationId = 2, Name = "Parking-N1", Type = "ParkingSpace", IsAvailable = true, HasTimeRestrictions = false },
            new ReservableObject { Id = 18, LocationId = 2, Name = "Parking-N2", Type = "ParkingSpace", IsAvailable = true, HasTimeRestrictions = false },
            new ReservableObject { Id = 19, LocationId = 2, Name = "Parking-N3", Type = "ParkingSpace", IsAvailable = false, HasTimeRestrictions = false },

            // East Side Location (Id = 3) - Desks
            new ReservableObject { Id = 20, LocationId = 3, Name = "Desk-E1", Type = "Desk", IsAvailable = true, HasTimeRestrictions = false },
            new ReservableObject { Id = 21, LocationId = 3, Name = "Desk-E2", Type = "Desk", IsAvailable = true, HasTimeRestrictions = false },
            new ReservableObject { Id = 22, LocationId = 3, Name = "Desk-E3", Type = "Desk", IsAvailable = true, HasTimeRestrictions = false },
            new ReservableObject { Id = 23, LocationId = 3, Name = "Desk-E4", Type = "Desk", IsAvailable = true, HasTimeRestrictions = true, AvailableFrom = new TimeOnly(7, 0), AvailableUntil = new TimeOnly(19, 0), DaysOfWeek = "Mon,Tue,Wed,Thu,Fri,Sat" },
            new ReservableObject { Id = 24, LocationId = 3, Name = "Desk-E5", Type = "Desk", IsAvailable = false, HasTimeRestrictions = false },
            new ReservableObject { Id = 25, LocationId = 3, Name = "Desk-E6", Type = "Desk", IsAvailable = true, HasTimeRestrictions = false },
            
            // East Side Location - Parking Spaces
            new ReservableObject { Id = 26, LocationId = 3, Name = "Parking-E1", Type = "ParkingSpace", IsAvailable = true, HasTimeRestrictions = false },
            new ReservableObject { Id = 27, LocationId = 3, Name = "Parking-E2", Type = "ParkingSpace", IsAvailable = true, HasTimeRestrictions = false },
            new ReservableObject { Id = 28, LocationId = 3, Name = "Parking-E3", Type = "ParkingSpace", IsAvailable = true, HasTimeRestrictions = true, AvailableFrom = new TimeOnly(6, 0), AvailableUntil = new TimeOnly(20, 0), DaysOfWeek = "Mon,Tue,Wed,Thu,Fri" },
            new ReservableObject { Id = 29, LocationId = 3, Name = "Parking-E4", Type = "ParkingSpace", IsAvailable = false, HasTimeRestrictions = false },
            new ReservableObject { Id = 30, LocationId = 3, Name = "Parking-E5", Type = "ParkingSpace", IsAvailable = true, HasTimeRestrictions = false }
        );

        // Seed sample reservations
        modelBuilder.Entity<Reservation>().HasData(
            new Reservation { Id = 1, ReservableObjectId = 1, Username = "john.doe", StartTime = new DateTime(2025, 10, 3, 9, 0, 0), EndTime = new DateTime(2025, 10, 3, 17, 0, 0), Status = "Active", CreatedAt = new DateTime(2025, 10, 1, 14, 30, 0) },
            new Reservation { Id = 2, ReservableObjectId = 8, Username = "jane.smith", StartTime = new DateTime(2025, 10, 3, 8, 0, 0), EndTime = new DateTime(2025, 10, 3, 18, 0, 0), Status = "Active", CreatedAt = new DateTime(2025, 10, 1, 15, 0, 0) },
            new Reservation { Id = 3, ReservableObjectId = 12, Username = "bob.wilson", StartTime = new DateTime(2025, 10, 4, 10, 0, 0), EndTime = new DateTime(2025, 10, 4, 14, 0, 0), Status = "Active", CreatedAt = new DateTime(2025, 10, 2, 9, 0, 0) },
            new Reservation { Id = 4, ReservableObjectId = 20, Username = "alice.brown", StartTime = new DateTime(2025, 10, 2, 13, 0, 0), EndTime = new DateTime(2025, 10, 2, 17, 0, 0), Status = "Active", CreatedAt = new DateTime(2025, 10, 1, 10, 0, 0) },
            new Reservation { Id = 5, ReservableObjectId = 26, Username = "john.doe", StartTime = new DateTime(2025, 10, 5, 7, 0, 0), EndTime = new DateTime(2025, 10, 5, 19, 0, 0), Status = "Active", CreatedAt = new DateTime(2025, 10, 2, 8, 0, 0) },
            new Reservation { Id = 6, ReservableObjectId = 2, Username = "jane.smith", StartTime = new DateTime(2025, 9, 30, 9, 0, 0), EndTime = new DateTime(2025, 9, 30, 17, 0, 0), Status = "Completed", CreatedAt = new DateTime(2025, 9, 29, 14, 0, 0) },
            new Reservation { Id = 7, ReservableObjectId = 17, Username = "bob.wilson", StartTime = new DateTime(2025, 9, 28, 8, 0, 0), EndTime = new DateTime(2025, 9, 28, 18, 0, 0), Status = "Completed", CreatedAt = new DateTime(2025, 9, 27, 16, 0, 0) },
            new Reservation { Id = 8, ReservableObjectId = 5, Username = "alice.brown", StartTime = new DateTime(2025, 10, 6, 9, 0, 0), EndTime = new DateTime(2025, 10, 6, 12, 0, 0), Status = "Active", CreatedAt = new DateTime(2025, 10, 2, 11, 0, 0) }
        );
    }
}
