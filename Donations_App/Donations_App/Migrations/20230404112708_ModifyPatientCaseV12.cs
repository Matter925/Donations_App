using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DonationsApp.Migrations
{
    /// <inheritdoc />
    public partial class ModifyPatientCaseV12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Limit",
                table: "PatientsCases",
                newName: "Rate");

            migrationBuilder.AddColumn<int>(
                name: "LimitTime",
                table: "PatientsCases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LimitTime",
                table: "PatientsCases");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "PatientsCases",
                newName: "Limit");
        }
    }
}
