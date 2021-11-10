using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentHouse.Data.Migrations
{
    public partial class uploadimageroom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubImageOfRoom",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomHouseId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubImageOfRoom", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubImageOfRoom_roomHouses_RoomHouseId",
                        column: x => x.RoomHouseId,
                        principalTable: "roomHouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubImageOfRoom_RoomHouseId",
                table: "SubImageOfRoom",
                column: "RoomHouseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubImageOfRoom");
        }
    }
}
