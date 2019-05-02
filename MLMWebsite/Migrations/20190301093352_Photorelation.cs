using Microsoft.EntityFrameworkCore.Migrations;

namespace MLMWebsite.Migrations
{
    public partial class Photorelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserAssets_UserId",
                table: "UserAssets");

            migrationBuilder.DropIndex(
                name: "IX_BarCodes_UserId",
                table: "BarCodes");

            migrationBuilder.DropIndex(
                name: "IX_AddressProofs_UserId",
                table: "AddressProofs");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssets_UserId",
                table: "UserAssets",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BarCodes_UserId",
                table: "BarCodes",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AddressProofs_UserId",
                table: "AddressProofs",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserAssets_UserId",
                table: "UserAssets");

            migrationBuilder.DropIndex(
                name: "IX_BarCodes_UserId",
                table: "BarCodes");

            migrationBuilder.DropIndex(
                name: "IX_AddressProofs_UserId",
                table: "AddressProofs");

            migrationBuilder.CreateIndex(
                name: "IX_UserAssets_UserId",
                table: "UserAssets",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BarCodes_UserId",
                table: "BarCodes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressProofs_UserId",
                table: "AddressProofs",
                column: "UserId");
        }
    }
}
