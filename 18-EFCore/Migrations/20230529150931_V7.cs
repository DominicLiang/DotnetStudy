using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _18_EFCore.Migrations
{
    /// <inheritdoc />
    public partial class V7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "D",
                table: "T_Books",
                type: "TEXT",
                nullable: false,
                defaultValue: "HelloWrold",
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_T_Books_Title",
                table: "T_Books",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_T_Books_Title_AuthorName",
                table: "T_Books",
                columns: new[] { "Title", "AuthorName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_T_Books_Title",
                table: "T_Books");

            migrationBuilder.DropIndex(
                name: "IX_T_Books_Title_AuthorName",
                table: "T_Books");

            migrationBuilder.AlterColumn<string>(
                name: "D",
                table: "T_Books",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "HelloWrold");
        }
    }
}
