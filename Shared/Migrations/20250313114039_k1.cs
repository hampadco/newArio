using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class k1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "WithdrawalRequests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalRequests_TransactionId",
                table: "WithdrawalRequests",
                column: "TransactionId",
                unique: true,
                filter: "[TransactionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_WithdrawalRequests_Transactions_TransactionId",
                table: "WithdrawalRequests",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WithdrawalRequests_Transactions_TransactionId",
                table: "WithdrawalRequests");

            migrationBuilder.DropIndex(
                name: "IX_WithdrawalRequests_TransactionId",
                table: "WithdrawalRequests");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "WithdrawalRequests");
        }
    }
}
