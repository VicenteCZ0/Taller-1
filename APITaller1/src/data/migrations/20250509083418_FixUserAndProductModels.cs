using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APITaller1.data.migrations
{
    /// <inheritdoc />
    public partial class FixUserAndProductModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Status_StatusID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Status_StatusID1",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_StatusID1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "StatusID1",
                table: "Products");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Status_StatusID",
                table: "Products",
                column: "StatusID",
                principalTable: "Status",
                principalColumn: "StatusID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Status_StatusID",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "StatusID1",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_StatusID1",
                table: "Products",
                column: "StatusID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Status_StatusID",
                table: "Products",
                column: "StatusID",
                principalTable: "Status",
                principalColumn: "StatusID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Status_StatusID1",
                table: "Products",
                column: "StatusID1",
                principalTable: "Status",
                principalColumn: "StatusID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
