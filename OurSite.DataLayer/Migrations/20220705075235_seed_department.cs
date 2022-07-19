using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurSite.DataLayer.Migrations
{
    public partial class seed_department : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "departments",
                columns: new[] { "Id", "CreateDate", "DepartmentName", "DepartmentTitle", "IsRemove", "LastUpdate" },
                values: new object[] { 1L, new DateTime(2022, 7, 5, 12, 22, 35, 825, DateTimeKind.Local).AddTicks(1847), "Financial", "بخش مالی", false, new DateTime(2022, 7, 5, 12, 22, 35, 825, DateTimeKind.Local).AddTicks(1881) });

            migrationBuilder.InsertData(
                table: "departments",
                columns: new[] { "Id", "CreateDate", "DepartmentName", "DepartmentTitle", "IsRemove", "LastUpdate" },
                values: new object[] { 2L, new DateTime(2022, 7, 5, 12, 22, 35, 825, DateTimeKind.Local).AddTicks(1883), "Technical support", "پشتیبانی فنی", false, new DateTime(2022, 7, 5, 12, 22, 35, 825, DateTimeKind.Local).AddTicks(1885) });

            migrationBuilder.InsertData(
                table: "departments",
                columns: new[] { "Id", "CreateDate", "DepartmentName", "DepartmentTitle", "IsRemove", "LastUpdate" },
                values: new object[] { 3L, new DateTime(2022, 7, 5, 12, 22, 35, 825, DateTimeKind.Local).AddTicks(1887), "Other", "سایر", false, new DateTime(2022, 7, 5, 12, 22, 35, 825, DateTimeKind.Local).AddTicks(1889) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 3L);
        }
    }
}
