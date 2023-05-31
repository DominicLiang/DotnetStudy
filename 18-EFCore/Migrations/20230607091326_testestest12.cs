using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _18_EFCore.Migrations
{
    /// <inheritdoc />
    public partial class testestest12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shops_Geo_LoactionId",
                table: "shops");

            migrationBuilder.DropTable(
                name: "Geo");

            migrationBuilder.DropIndex(
                name: "IX_shops_LoactionId",
                table: "shops");

            migrationBuilder.DropColumn(
                name: "LoactionId",
                table: "shops");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "shops",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<double>(
                name: "Loaction_Latitude",
                table: "shops",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Loaction_Longitude",
                table: "shops",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Loaction_Latitude",
                table: "shops");

            migrationBuilder.DropColumn(
                name: "Loaction_Longitude",
                table: "shops");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "shops",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddColumn<long>(
                name: "LoactionId",
                table: "shops",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Geo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Geo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_shops_LoactionId",
                table: "shops",
                column: "LoactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_shops_Geo_LoactionId",
                table: "shops",
                column: "LoactionId",
                principalTable: "Geo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
