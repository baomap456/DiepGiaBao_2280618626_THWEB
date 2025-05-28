using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace THLapTrinhWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddIsVideoColumnToProductImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVideo",
                table: "ProductImages",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVideo",
                table: "ProductImages");
        }
    }
}
