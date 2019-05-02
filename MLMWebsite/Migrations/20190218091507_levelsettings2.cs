using Microsoft.EntityFrameworkCore.Migrations;

namespace MLMWebsite.Migrations
{
    public partial class levelsettings2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Admin",
                table: "LevelSettings",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Admin",
                table: "LevelSettings");
        }
    }
}
