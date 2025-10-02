using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workshop.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFloorPlanPositions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionX",
                table: "ReservableObjects",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PositionY",
                table: "ReservableObjects",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 100, 100 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 250, 100 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 400, 100 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 100, 250 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 250, 250 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 100, 400 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 250, 400 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 100, 550 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 250, 550 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 400, 550 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 550, 550 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 100, 100 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 250, 100 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 400, 100 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 100, 250 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 250, 250 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 100, 400 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 250, 400 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 400, 400 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 100, 100 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 250, 100 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 400, 100 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 100, 250 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 250, 250 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 400, 250 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 100, 400 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 250, 400 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 400, 400 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 550, 400 });

            migrationBuilder.UpdateData(
                table: "ReservableObjects",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "PositionX", "PositionY" },
                values: new object[] { 700, 400 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionX",
                table: "ReservableObjects");

            migrationBuilder.DropColumn(
                name: "PositionY",
                table: "ReservableObjects");
        }
    }
}
