using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentHouse.Data.Migrations
{
    public partial class Ko : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlImg",
                table: "roomHouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlImg",
                table: "roomHouses");
        }
    }
}
