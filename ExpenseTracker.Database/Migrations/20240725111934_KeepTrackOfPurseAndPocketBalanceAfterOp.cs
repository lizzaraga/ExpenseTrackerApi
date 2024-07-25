using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Database.Migrations
{
    /// <inheritdoc />
    public partial class KeepTrackOfPurseAndPocketBalanceAfterOp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "NextPocketBalance",
                table: "PursePocketTransfers",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NextPurseBalance",
                table: "PursePocketTransfers",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NextPurseBalance",
                table: "PurseIncomeHistories",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "FromPocketNextBalance",
                table: "PocketPocketTransfers",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ToPocketNextBalance",
                table: "PocketPocketTransfers",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "NextPocketBalance",
                table: "PocketExpenseHistories",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextPocketBalance",
                table: "PursePocketTransfers");

            migrationBuilder.DropColumn(
                name: "NextPurseBalance",
                table: "PursePocketTransfers");

            migrationBuilder.DropColumn(
                name: "NextPurseBalance",
                table: "PurseIncomeHistories");

            migrationBuilder.DropColumn(
                name: "FromPocketNextBalance",
                table: "PocketPocketTransfers");

            migrationBuilder.DropColumn(
                name: "ToPocketNextBalance",
                table: "PocketPocketTransfers");

            migrationBuilder.DropColumn(
                name: "NextPocketBalance",
                table: "PocketExpenseHistories");
        }
    }
}
