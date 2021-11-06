using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentHouse.Data.Migrations
{
    public partial class hieufix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_houses_AspNetUsers_ApplicationUserId",
                table: "houses");

            migrationBuilder.DropIndex(
                name: "IX_houses_ApplicationUserId",
                table: "houses");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "houses");

            migrationBuilder.CreateTable(
                name: "UserRent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomId = table.Column<int>(type: "int", nullable: false),
                    roomHouseId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRent_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRent_roomHouses_roomHouseId",
                        column: x => x.roomHouseId,
                        principalTable: "roomHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRent_ApplicationUserId",
                table: "UserRent",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRent_roomHouseId",
                table: "UserRent",
                column: "roomHouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRent");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "houses",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_houses_ApplicationUserId",
                table: "houses",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_houses_AspNetUsers_ApplicationUserId",
                table: "houses",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
