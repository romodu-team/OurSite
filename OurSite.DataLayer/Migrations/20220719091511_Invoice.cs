using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurSite.DataLayer.Migrations
{
    public partial class Invoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 19, 13, 45, 11, 349, DateTimeKind.Local).AddTicks(7666), new DateTime(2022, 7, 19, 13, 45, 11, 349, DateTimeKind.Local).AddTicks(7698) });

            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 19, 13, 45, 11, 349, DateTimeKind.Local).AddTicks(7701), new DateTime(2022, 7, 19, 13, 45, 11, 349, DateTimeKind.Local).AddTicks(7702) });

            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 19, 13, 45, 11, 349, DateTimeKind.Local).AddTicks(7704), new DateTime(2022, 7, 19, 13, 45, 11, 349, DateTimeKind.Local).AddTicks(7705) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
