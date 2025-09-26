using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stimpon.Community.Api.Migrations
{
    /// <inheritdoc />
    public partial class Migration_8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Answer",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "Comments");
        }
    }
}
