using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurSite.DataLayer.Migrations
{
    public partial class checkBox : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckBoxs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckBoxName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sectionName = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRemove = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckBoxs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemSelected",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConsultationFormId = table.Column<long>(type: "bigint", nullable: false),
                    CheckBoxId = table.Column<long>(type: "bigint", nullable: false),
                    CheckBoxsId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRemove = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSelected", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemSelected_CheckBoxs_CheckBoxsId",
                        column: x => x.CheckBoxsId,
                        principalTable: "CheckBoxs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemSelected_consultationRequest_ConsultationFormId",
                        column: x => x.ConsultationFormId,
                        principalTable: "consultationRequest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 16, 10, 28, 44, 926, DateTimeKind.Local).AddTicks(6275), new DateTime(2022, 7, 16, 10, 28, 44, 926, DateTimeKind.Local).AddTicks(6303) });

            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 16, 10, 28, 44, 926, DateTimeKind.Local).AddTicks(6305), new DateTime(2022, 7, 16, 10, 28, 44, 926, DateTimeKind.Local).AddTicks(6307) });

            migrationBuilder.UpdateData(
                table: "departments",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "CreateDate", "LastUpdate" },
                values: new object[] { new DateTime(2022, 7, 16, 10, 28, 44, 926, DateTimeKind.Local).AddTicks(6309), new DateTime(2022, 7, 16, 10, 28, 44, 926, DateTimeKind.Local).AddTicks(6310) });

            migrationBuilder.CreateIndex(
                name: "IX_ItemSelected_CheckBoxsId",
                table: "ItemSelected",
                column: "CheckBoxsId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSelected_ConsultationFormId",
                table: "ItemSelected",
                column: "ConsultationFormId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemSelected");

            migrationBuilder.DropTable(
                name: "CheckBoxs");

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
    }
}
