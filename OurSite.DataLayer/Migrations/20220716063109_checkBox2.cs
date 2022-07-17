using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurSite.DataLayer.Migrations
{
    public partial class checkBox2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSelected_CheckBoxs_CheckBoxsId",
                table: "ItemSelected");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSelected_consultationRequest_ConsultationFormId",
                table: "ItemSelected");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItemSelected",
                table: "ItemSelected");

            migrationBuilder.DropIndex(
                name: "IX_ItemSelected_CheckBoxsId",
                table: "ItemSelected");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckBoxs",
                table: "CheckBoxs");

            migrationBuilder.DropColumn(
                name: "CheckBoxsId",
                table: "ItemSelected");

            migrationBuilder.RenameTable(
                name: "ItemSelected",
                newName: "itemsSelecteds");

            migrationBuilder.RenameTable(
                name: "CheckBoxs",
                newName: "checkBoxes");

            migrationBuilder.RenameIndex(
                name: "IX_ItemSelected_ConsultationFormId",
                table: "itemsSelecteds",
                newName: "IX_itemsSelecteds_ConsultationFormId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_itemsSelecteds",
                table: "itemsSelecteds",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_checkBoxes",
                table: "checkBoxes",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_itemsSelecteds_CheckBoxId",
                table: "itemsSelecteds",
                column: "CheckBoxId");

            migrationBuilder.AddForeignKey(
                name: "FK_itemsSelecteds_checkBoxes_CheckBoxId",
                table: "itemsSelecteds",
                column: "CheckBoxId",
                principalTable: "checkBoxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_itemsSelecteds_consultationRequest_ConsultationFormId",
                table: "itemsSelecteds",
                column: "ConsultationFormId",
                principalTable: "consultationRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_itemsSelecteds_checkBoxes_CheckBoxId",
                table: "itemsSelecteds");

            migrationBuilder.DropForeignKey(
                name: "FK_itemsSelecteds_consultationRequest_ConsultationFormId",
                table: "itemsSelecteds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_itemsSelecteds",
                table: "itemsSelecteds");

            migrationBuilder.DropIndex(
                name: "IX_itemsSelecteds_CheckBoxId",
                table: "itemsSelecteds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_checkBoxes",
                table: "checkBoxes");

            migrationBuilder.RenameTable(
                name: "itemsSelecteds",
                newName: "ItemSelected");

            migrationBuilder.RenameTable(
                name: "checkBoxes",
                newName: "CheckBoxs");

            migrationBuilder.RenameIndex(
                name: "IX_itemsSelecteds_ConsultationFormId",
                table: "ItemSelected",
                newName: "IX_ItemSelected_ConsultationFormId");

            migrationBuilder.AddColumn<long>(
                name: "CheckBoxsId",
                table: "ItemSelected",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItemSelected",
                table: "ItemSelected",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckBoxs",
                table: "CheckBoxs",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSelected_CheckBoxs_CheckBoxsId",
                table: "ItemSelected",
                column: "CheckBoxsId",
                principalTable: "CheckBoxs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSelected_consultationRequest_ConsultationFormId",
                table: "ItemSelected",
                column: "ConsultationFormId",
                principalTable: "consultationRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
