using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoRoute.DataCollection.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWasteBinEntityy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FillLevel",
                table: "WasteBins",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FillLevel",
                table: "WasteBins");
        }
    }
}
