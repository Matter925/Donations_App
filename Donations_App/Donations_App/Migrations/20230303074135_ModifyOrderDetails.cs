using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DonationsApp.Migrations
{
    /// <inheritdoc />
    public partial class ModifyOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrdersDetails_OrderId",
                table: "OrdersDetails");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDetails_OrderId",
                table: "OrdersDetails",
                column: "OrderId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrdersDetails_OrderId",
                table: "OrdersDetails");

            migrationBuilder.CreateIndex(
                name: "IX_OrdersDetails_OrderId",
                table: "OrdersDetails",
                column: "OrderId");
        }
    }
}
