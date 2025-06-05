using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoRoute.DataCollection.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "estimatedFillLevel",
                table: "WasteBins",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "estimatedFillLevel",
                table: "WasteBins");
        }
    }
}
