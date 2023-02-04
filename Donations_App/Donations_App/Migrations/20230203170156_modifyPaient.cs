using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DonationsApp.Migrations
{
    /// <inheritdoc />
    public partial class modifyPaient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Image",
                table: "PatientsCases",
                type: "varbinary(max)",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Image",
                table: "PatientsCases",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");
        }
    }
}
