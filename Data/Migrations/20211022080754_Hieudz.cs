using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentHouse.Data.Migrations
{
    public partial class Hieudz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "agreements",
                columns: table => new
                {
                    AgreementID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserRentId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MonneyForMonth = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agreements", x => x.AgreementID);
                    table.ForeignKey(
                        name: "FK_agreements_UserRent_UserRentId",
                        column: x => x.UserRentId,
                        principalTable: "UserRent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserRentId = table.Column<int>(type: "int", nullable: false),
                    dateTimeCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deposit = table.Column<double>(type: "float", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transactions_UserRent_UserRentId",
                        column: x => x.UserRentId,
                        principalTable: "UserRent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_agreements_UserRentId",
                table: "agreements",
                column: "UserRentId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_UserRentId",
                table: "transactions",
                column: "UserRentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agreements");

            migrationBuilder.DropTable(
                name: "transactions");
        }
    }
}
