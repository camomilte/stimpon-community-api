using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stimpon.Community.Api.Migrations
{
    /// <inheritdoc />
    public partial class Migration_5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Users_UserId",
                table: "Threads");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Threads",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Threads_UserId",
                table: "Threads",
                newName: "IX_Threads_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Users_OwnerId",
                table: "Threads",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Users_OwnerId",
                table: "Threads");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Threads",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Threads_OwnerId",
                table: "Threads",
                newName: "IX_Threads_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Users_UserId",
                table: "Threads",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
