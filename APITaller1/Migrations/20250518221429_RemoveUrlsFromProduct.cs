using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APITaller1.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUrlsFromProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Urls",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Urls",
                table: "Products",
                type: "TEXT",
                nullable: true);
        }
    }
}
