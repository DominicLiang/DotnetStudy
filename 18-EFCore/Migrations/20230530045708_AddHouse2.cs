using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _18_EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AddHouse2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_houses",
                table: "houses");

            migrationBuilder.RenameTable(
                name: "houses",
                newName: "T_Houses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_T_Houses",
                table: "T_Houses",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_T_Houses",
                table: "T_Houses");

            migrationBuilder.RenameTable(
                name: "T_Houses",
                newName: "houses");

            migrationBuilder.AddPrimaryKey(
                name: "PK_houses",
                table: "houses",
                column: "Id");
        }
    }
}
