using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurSite.DataLayer.Migrations
{
    public partial class addContactWithUs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contactWithUs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserFirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserLastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRemove = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contactWithUs", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contactWithUs");
        }
    }
}
