using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workshop.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerUsername",
                table: "ReservableObjects",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccessRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReservableObjectId = table.Column<int>(type: "INTEGER", nullable: false),
                    RequesterUsername = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RequestedStartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RequestedEndTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RespondedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ResponseMessage = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessRequests_ReservableObjects_ReservableObjectId",
                        column: x => x.ReservableObjectId,
                        principalTable: "ReservableObjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 1,
                column: "OwnerUsername",
                value: "john.doe");

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 2,
                column: "OwnerUsername",
                value: "jane.smith");

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 3,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 4,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 5,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 6,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 7,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 8,
                column: "OwnerUsername",
                value: "bob.wilson");

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 9,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 10,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 11,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 12,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 13,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 14,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 15,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 16,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 17,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 18,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 19,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 20,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 21,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 22,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 23,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 24,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 25,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 26,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 27,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 28,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 29,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 30,
                column: "OwnerUsername",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequests_RequesterUsername",
                table: "AccessRequests",
                column: "RequesterUsername");

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequests_ReservableObjectId",
                table: "AccessRequests",
                column: "ReservableObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessRequests_Status",
                table: "AccessRequests",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccessRequests");

            migrationBuilder.DropColumn(
                name: "OwnerUsername",
                table: "ReservableObjects");
        }
    }
}
