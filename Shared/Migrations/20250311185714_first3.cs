using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class first3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepositRequests_Cards_CardsId",
                table: "DepositRequests");

            migrationBuilder.DropIndex(
                name: "IX_DepositRequests_CardsId",
                table: "DepositRequests");

            migrationBuilder.DropColumn(
                name: "CardsId",
                table: "DepositRequests");

            migrationBuilder.CreateIndex(
                name: "IX_DepositRequests_CardId",
                table: "DepositRequests",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepositRequests_Cards_CardId",
                table: "DepositRequests",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepositRequests_Cards_CardId",
                table: "DepositRequests");

            migrationBuilder.DropIndex(
                name: "IX_DepositRequests_CardId",
                table: "DepositRequests");

            migrationBuilder.AddColumn<int>(
                name: "CardsId",
                table: "DepositRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DepositRequests_CardsId",
                table: "DepositRequests",
                column: "CardsId");

            migrationBuilder.AddForeignKey(
                name: "FK_DepositRequests_Cards_CardsId",
                table: "DepositRequests",
                column: "CardsId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
