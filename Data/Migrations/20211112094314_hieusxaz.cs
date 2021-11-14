using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentHouse.Data.Migrations
{
    public partial class hieusxaz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRent_roomHouses_roomHouseId",
                table: "UserRent");

            migrationBuilder.RenameColumn(
                name: "roomHouseId",
                table: "UserRent",
                newName: "RoomHouseId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRent_roomHouseId",
                table: "UserRent",
                newName: "IX_UserRent_RoomHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRent_roomHouses_RoomHouseId",
                table: "UserRent",
                column: "RoomHouseId",
                principalTable: "roomHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRent_roomHouses_RoomHouseId",
                table: "UserRent");

            migrationBuilder.RenameColumn(
                name: "RoomHouseId",
                table: "UserRent",
                newName: "roomHouseId");

            migrationBuilder.RenameIndex(
                name: "IX_UserRent_RoomHouseId",
                table: "UserRent",
                newName: "IX_UserRent_roomHouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRent_roomHouses_roomHouseId",
                table: "UserRent",
                column: "roomHouseId",
                principalTable: "roomHouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
