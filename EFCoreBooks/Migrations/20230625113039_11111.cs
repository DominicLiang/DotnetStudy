using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreBooks.Migrations
{
    /// <inheritdoc />
    public partial class _11111 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PeopleId",
                table: "T_Books");

            migrationBuilder.AddColumn<long>(
                name: "BookId",
                table: "Peoples",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Peoples_BookId",
                table: "Peoples",
                column: "BookId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Peoples_T_Books_BookId",
                table: "Peoples",
                column: "BookId",
                principalTable: "T_Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Peoples_T_Books_BookId",
                table: "Peoples");

            migrationBuilder.DropIndex(
                name: "IX_Peoples_BookId",
                table: "Peoples");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Peoples");

            migrationBuilder.AddColumn<long>(
                name: "PeopleId",
                table: "T_Books",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
