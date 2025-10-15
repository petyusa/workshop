using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Workshop.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReservableObjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_ReservableObjects_ReservableObjectId",
                        column: x => x.ReservableObjectId,
                        principalTable: "ReservableObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "Id", "CreatedAt", "EndTime", "ReservableObjectId", "StartTime", "Status", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 10, 1, 14, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 3, 17, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(2025, 10, 3, 9, 0, 0, 0, DateTimeKind.Unspecified), "Active", "john.doe" },
                    { 2, new DateTime(2025, 10, 1, 15, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 3, 18, 0, 0, 0, DateTimeKind.Unspecified), 8, new DateTime(2025, 10, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), "Active", "jane.smith" },
                    { 3, new DateTime(2025, 10, 2, 9, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 4, 14, 0, 0, 0, DateTimeKind.Unspecified), 12, new DateTime(2025, 10, 4, 10, 0, 0, 0, DateTimeKind.Unspecified), "Active", "bob.wilson" },
                    { 4, new DateTime(2025, 10, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 2, 17, 0, 0, 0, DateTimeKind.Unspecified), 20, new DateTime(2025, 10, 2, 13, 0, 0, 0, DateTimeKind.Unspecified), "Active", "alice.brown" },
                    { 5, new DateTime(2025, 10, 2, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 5, 19, 0, 0, 0, DateTimeKind.Unspecified), 26, new DateTime(2025, 10, 5, 7, 0, 0, 0, DateTimeKind.Unspecified), "Active", "john.doe" },
                    { 6, new DateTime(2025, 9, 29, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 30, 17, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(2025, 9, 30, 9, 0, 0, 0, DateTimeKind.Unspecified), "Completed", "jane.smith" },
                    { 7, new DateTime(2025, 9, 27, 16, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 9, 28, 18, 0, 0, 0, DateTimeKind.Unspecified), 17, new DateTime(2025, 9, 28, 8, 0, 0, 0, DateTimeKind.Unspecified), "Completed", "bob.wilson" },
                    { 8, new DateTime(2025, 10, 2, 11, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 6, 12, 0, 0, 0, DateTimeKind.Unspecified), 5, new DateTime(2025, 10, 6, 9, 0, 0, 0, DateTimeKind.Unspecified), "Active", "alice.brown" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservableObjectId",
                table: "Reservations",
                column: "ReservableObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_StartTime",
                table: "Reservations",
                column: "StartTime");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Status",
                table: "Reservations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Username",
                table: "Reservations",
                column: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");
        }
    }
}
