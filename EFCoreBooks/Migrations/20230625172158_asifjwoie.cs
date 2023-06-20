using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreBooks.Migrations
{
    /// <inheritdoc />
    public partial class asifjwoie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Peoples_BookId",
                table: "Peoples");

            migrationBuilder.CreateIndex(
                name: "IX_Peoples_BookId",
                table: "Peoples",
                column: "BookId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Peoples_BookId",
                table: "Peoples");

            migrationBuilder.CreateIndex(
                name: "IX_Peoples_BookId",
                table: "Peoples",
                column: "BookId");
        }
    }
}
