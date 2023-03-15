using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DonationsApp.Migrations
{
    /// <inheritdoc />
    public partial class changeRequestStaus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "Rejected",
                table: "Requests");

            migrationBuilder.AddColumn<string>(
                name: "RequestStatus",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "wait");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestStatus",
                table: "Requests");

            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Rejected",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
