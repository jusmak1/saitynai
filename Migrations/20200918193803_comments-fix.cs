using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialAPI.Migrations
{
    public partial class commentsfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_CreatorEmail",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CreatorEmail",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CreatorEmail",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "PostedBy",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostedBy",
                table: "Comments",
                column: "PostedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_PostedBy",
                table: "Comments",
                column: "PostedBy",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_PostedBy",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostedBy",
                table: "Comments");

            migrationBuilder.AlterColumn<string>(
                name: "PostedBy",
                table: "Comments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "CreatorEmail",
                table: "Comments",
                type: "nvarchar(255)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CreatorEmail",
                table: "Comments",
                column: "CreatorEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_CreatorEmail",
                table: "Comments",
                column: "CreatorEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
