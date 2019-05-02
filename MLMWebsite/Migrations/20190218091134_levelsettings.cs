using Microsoft.EntityFrameworkCore.Migrations;

namespace MLMWebsite.Migrations
{
    public partial class levelsettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceperHead",
                table: "LevelSettings");

            migrationBuilder.AddColumn<double>(
                name: "Level1",
                table: "LevelSettings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Level10",
                table: "LevelSettings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Level2",
                table: "LevelSettings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Level3",
                table: "LevelSettings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Level4",
                table: "LevelSettings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Level5",
                table: "LevelSettings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Level6",
                table: "LevelSettings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Level7",
                table: "LevelSettings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Level8",
                table: "LevelSettings",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Level9",
                table: "LevelSettings",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level1",
                table: "LevelSettings");

            migrationBuilder.DropColumn(
                name: "Level10",
                table: "LevelSettings");

            migrationBuilder.DropColumn(
                name: "Level2",
                table: "LevelSettings");

            migrationBuilder.DropColumn(
                name: "Level3",
                table: "LevelSettings");

            migrationBuilder.DropColumn(
                name: "Level4",
                table: "LevelSettings");

            migrationBuilder.DropColumn(
                name: "Level5",
                table: "LevelSettings");

            migrationBuilder.DropColumn(
                name: "Level6",
                table: "LevelSettings");

            migrationBuilder.DropColumn(
                name: "Level7",
                table: "LevelSettings");

            migrationBuilder.DropColumn(
                name: "Level8",
                table: "LevelSettings");

            migrationBuilder.DropColumn(
                name: "Level9",
                table: "LevelSettings");

            migrationBuilder.AddColumn<string>(
                name: "PriceperHead",
                table: "LevelSettings",
                nullable: true);
        }
    }
}
