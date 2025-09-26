using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stimpon.Community.Api.Migrations
{
    /// <inheritdoc />
    public partial class Migration_7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Threads_ParentThreadId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Users_OwnerId",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_ParentThreadId",
                table: "Comments",
                newName: "IX_Comments_ParentThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_OwnerId",
                table: "Comments",
                newName: "IX_Comments_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Threads_ParentThreadId",
                table: "Comments",
                column: "ParentThreadId",
                principalTable: "Threads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Users_OwnerId",
                table: "Comments",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Threads_ParentThreadId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Users_OwnerId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_ParentThreadId",
                table: "Comment",
                newName: "IX_Comment_ParentThreadId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_OwnerId",
                table: "Comment",
                newName: "IX_Comment_OwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Threads_ParentThreadId",
                table: "Comment",
                column: "ParentThreadId",
                principalTable: "Threads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Users_OwnerId",
                table: "Comment",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
