using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentHouse.Data.Migrations
{
    public partial class HieuHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistoryPay",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomHouseId = table.Column<int>(type: "int", nullable: false),
                    PricePay = table.Column<double>(type: "float", nullable: false),
                    TimePay = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryPay", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryPay_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoryPay_roomHouses_RoomHouseId",
                        column: x => x.RoomHouseId,
                        principalTable: "roomHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoryPay_ApplicationUserId",
                table: "HistoryPay",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoryPay_RoomHouseId",
                table: "HistoryPay",
                column: "RoomHouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoryPay");
        }
    }
}
