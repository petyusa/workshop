using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Workshop.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddReservableObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReservableObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasTimeRestrictions = table.Column<bool>(type: "INTEGER", nullable: false),
                    AvailableFrom = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    AvailableUntil = table.Column<TimeOnly>(type: "TEXT", nullable: true),
                    DaysOfWeek = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservableObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservableObjects_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ReservableObjects",
                columns: new[] { "Id", "AvailableFrom", "AvailableUntil", "DaysOfWeek", "HasTimeRestrictions", "IsAvailable", "LocationId", "Name", "Type" },
                values: new object[,]
                {
                    { 1, null, null, null, false, true, 1, "Desk-A1", "Desk" },
                    { 2, null, null, null, false, true, 1, "Desk-A2", "Desk" },
                    { 3, null, null, null, false, false, 1, "Desk-A3", "Desk" },
                    { 4, new TimeOnly(8, 0, 0), new TimeOnly(18, 0, 0), "Mon,Tue,Wed,Thu,Fri", true, true, 1, "Desk-B1", "Desk" },
                    { 5, new TimeOnly(8, 0, 0), new TimeOnly(18, 0, 0), "Mon,Tue,Wed,Thu,Fri", true, true, 1, "Desk-B2", "Desk" },
                    { 6, null, null, null, false, true, 1, "Desk-C1", "Desk" },
                    { 7, null, null, null, false, true, 1, "Desk-C2", "Desk" },
                    { 8, null, null, null, false, true, 1, "Parking-P1", "ParkingSpace" },
                    { 9, null, null, null, false, false, 1, "Parking-P2", "ParkingSpace" },
                    { 10, new TimeOnly(6, 0, 0), new TimeOnly(22, 0, 0), "Mon,Tue,Wed,Thu,Fri,Sat,Sun", true, true, 1, "Parking-P3", "ParkingSpace" },
                    { 11, null, null, null, false, true, 1, "Parking-P4", "ParkingSpace" },
                    { 12, null, null, null, false, true, 2, "Desk-N1", "Desk" },
                    { 13, null, null, null, false, true, 2, "Desk-N2", "Desk" },
                    { 14, null, null, null, false, true, 2, "Desk-N3", "Desk" },
                    { 15, null, null, null, false, false, 2, "Desk-N4", "Desk" },
                    { 16, new TimeOnly(9, 0, 0), new TimeOnly(17, 0, 0), "Mon,Tue,Wed,Thu,Fri", true, true, 2, "Desk-N5", "Desk" },
                    { 17, null, null, null, false, true, 2, "Parking-N1", "ParkingSpace" },
                    { 18, null, null, null, false, true, 2, "Parking-N2", "ParkingSpace" },
                    { 19, null, null, null, false, false, 2, "Parking-N3", "ParkingSpace" },
                    { 20, null, null, null, false, true, 3, "Desk-E1", "Desk" },
                    { 21, null, null, null, false, true, 3, "Desk-E2", "Desk" },
                    { 22, null, null, null, false, true, 3, "Desk-E3", "Desk" },
                    { 23, new TimeOnly(7, 0, 0), new TimeOnly(19, 0, 0), "Mon,Tue,Wed,Thu,Fri,Sat", true, true, 3, "Desk-E4", "Desk" },
                    { 24, null, null, null, false, false, 3, "Desk-E5", "Desk" },
                    { 25, null, null, null, false, true, 3, "Desk-E6", "Desk" },
                    { 26, null, null, null, false, true, 3, "Parking-E1", "ParkingSpace" },
                    { 27, null, null, null, false, true, 3, "Parking-E2", "ParkingSpace" },
                    { 28, new TimeOnly(6, 0, 0), new TimeOnly(20, 0, 0), "Mon,Tue,Wed,Thu,Fri", true, true, 3, "Parking-E3", "ParkingSpace" },
                    { 29, null, null, null, false, false, 3, "Parking-E4", "ParkingSpace" },
                    { 30, null, null, null, false, true, 3, "Parking-E5", "ParkingSpace" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservableObjects_LocationId",
                table: "ReservableObjects",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservableObjects");
        }
    }
}
