using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentHouse.Data.Migrations
{
    public partial class confiem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConfirmHouse",
                table: "houses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmHouse",
                table: "houses");
        }
    }
}
