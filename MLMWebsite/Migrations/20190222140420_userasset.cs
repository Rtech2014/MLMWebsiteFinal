using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MLMWebsite.Migrations
{
    public partial class userasset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressProof",
                table: "UserAssets");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "UserAssets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "AddressProof",
                table: "UserAssets",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Barcode",
                table: "UserAssets",
                nullable: true);
        }
    }
}
