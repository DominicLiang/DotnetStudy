using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _18_EFCore.Migrations
{
    /// <inheritdoc />
    public partial class v9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PubTime",
                table: "T_Books",
                type: "DATETIME",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "PubTime",
                table: "T_Books",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME");
        }
    }
}
