using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DonationsApp.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Request_Status",
                table: "Requests",
                newName: "Rejected");

            migrationBuilder.AddColumn<bool>(
                name: "Accepted",
                table: "Requests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accepted",
                table: "Requests");

            migrationBuilder.RenameColumn(
                name: "Rejected",
                table: "Requests",
                newName: "Request_Status");
        }
    }
}
