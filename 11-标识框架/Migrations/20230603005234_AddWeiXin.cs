using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _11_标识框架.Migrations
{
    /// <inheritdoc />
    public partial class AddWeiXin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WeiXinAccount",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeiXinAccount",
                table: "AspNetUsers");
        }
    }
}
