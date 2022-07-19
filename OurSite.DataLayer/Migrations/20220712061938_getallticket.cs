using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurSite.DataLayer.Migrations
{
    public partial class getallticket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 12, 10, 49, 38, 568, DateTimeKind.Local).AddTicks(4407), new DateTime(2022, 7, 12, 10, 49, 38, 568, DateTimeKind.Local).AddTicks(4441) });

            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 12, 10, 49, 38, 568, DateTimeKind.Local).AddTicks(4445), new DateTime(2022, 7, 12, 10, 49, 38, 568, DateTimeKind.Local).AddTicks(4447) });

            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 12, 10, 49, 38, 568, DateTimeKind.Local).AddTicks(4450), new DateTime(2022, 7, 12, 10, 49, 38, 568, DateTimeKind.Local).AddTicks(4451) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 5, 12, 22, 35, 825, DateTimeKind.Local).AddTicks(1847), new DateTime(2022, 7, 5, 12, 22, 35, 825, DateTimeKind.Local).AddTicks(1881) });

            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 5, 12, 22, 35, 825, DateTimeKind.Local).AddTicks(1883), new DateTime(2022, 7, 5, 12, 22, 35, 825, DateTimeKind.Local).AddTicks(1885) });

            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 5, 12, 22, 35, 825, DateTimeKind.Local).AddTicks(1887), new DateTime(2022, 7, 5, 12, 22, 35, 825, DateTimeKind.Local).AddTicks(1889) });
        }
    }
}
