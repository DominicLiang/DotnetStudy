using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _18_EFCore.Migrations
{
    /// <inheritdoc />
    public partial class like2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Like",
                table: "Like");

            migrationBuilder.RenameTable(
                name: "Like",
                newName: "likes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_likes",
                table: "likes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_likes",
                table: "likes");

            migrationBuilder.RenameTable(
                name: "likes",
                newName: "Like");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Like",
                table: "Like",
                column: "Id");
        }
    }
}
