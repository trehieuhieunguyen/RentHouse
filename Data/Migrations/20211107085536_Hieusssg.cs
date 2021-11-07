using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentHouse.Data.Migrations
{
    public partial class Hieusssg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "messageRooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomHouseId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messageRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_messageRooms_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_messageRooms_roomHouses_RoomHouseId",
                        column: x => x.RoomHouseId,
                        principalTable: "roomHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_messageRooms_ApplicationUserId",
                table: "messageRooms",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_messageRooms_RoomHouseId",
                table: "messageRooms",
                column: "RoomHouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "messageRooms");
        }
    }
}
