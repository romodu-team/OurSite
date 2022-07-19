using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurSite.DataLayer.Migrations
{
    public partial class Payment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "ticketMessages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 19, 8, 59, 59, 774, DateTimeKind.Local).AddTicks(5707), new DateTime(2022, 7, 19, 8, 59, 59, 774, DateTimeKind.Local).AddTicks(5751) });

            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 19, 8, 59, 59, 774, DateTimeKind.Local).AddTicks(5757), new DateTime(2022, 7, 19, 8, 59, 59, 774, DateTimeKind.Local).AddTicks(5761) });

            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 19, 8, 59, 59, 774, DateTimeKind.Local).AddTicks(5765), new DateTime(2022, 7, 19, 8, 59, 59, 774, DateTimeKind.Local).AddTicks(5768) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "ticketMessages");

            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 16, 11, 1, 9, 66, DateTimeKind.Local).AddTicks(9773), new DateTime(2022, 7, 16, 11, 1, 9, 66, DateTimeKind.Local).AddTicks(9802) });

            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 16, 11, 1, 9, 66, DateTimeKind.Local).AddTicks(9805), new DateTime(2022, 7, 16, 11, 1, 9, 66, DateTimeKind.Local).AddTicks(9806) });

            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 16, 11, 1, 9, 66, DateTimeKind.Local).AddTicks(9808), new DateTime(2022, 7, 16, 11, 1, 9, 66, DateTimeKind.Local).AddTicks(9809) });
        }
    }
}
