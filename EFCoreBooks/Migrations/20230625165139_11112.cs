using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCoreBooks.Migrations
{
    /// <inheritdoc />
    public partial class _11112 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Peoples_BookId",
                table: "Peoples");

            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "T_Books");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "T_Books");

            migrationBuilder.DropColumn(
                name: "PubDate",
                table: "T_Books");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "T_Books",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Peoples",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Peoples",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Peoples_BookId",
                table: "Peoples",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Peoples_BookId",
                table: "Peoples");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Peoples");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "T_Books",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Peoples",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "T_Books",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Price",
                table: "T_Books",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PubDate",
                table: "T_Books",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Peoples_BookId",
                table: "Peoples",
                column: "BookId",
                unique: true);
        }
    }
}
