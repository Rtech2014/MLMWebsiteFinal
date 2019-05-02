using Microsoft.EntityFrameworkCore.Migrations;

namespace MLMWebsite.Migrations
{
    public partial class userstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserStatus");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserStatus",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "ApprovalCount",
                table: "UserStatus",
                newName: "ProofID");

            migrationBuilder.AddColumn<string>(
                name: "ApproverID",
                table: "UserStatus",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserStatus_ProofID",
                table: "UserStatus",
                column: "ProofID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStatus_Proof_ProofID",
                table: "UserStatus",
                column: "ProofID",
                principalTable: "Proof",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStatus_Proof_ProofID",
                table: "UserStatus");

            migrationBuilder.DropIndex(
                name: "IX_UserStatus_ProofID",
                table: "UserStatus");

            migrationBuilder.DropColumn(
                name: "ApproverID",
                table: "UserStatus");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "UserStatus",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ProofID",
                table: "UserStatus",
                newName: "ApprovalCount");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "UserStatus",
                nullable: false,
                defaultValue: false);
        }
    }
}
