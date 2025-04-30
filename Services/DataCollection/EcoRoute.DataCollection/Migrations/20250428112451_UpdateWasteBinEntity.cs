using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoRoute.DataCollection.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWasteBinEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "WasteBins");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "WasteBins",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "WasteBins",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "WasteBins",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "WasteBins");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "WasteBins");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "WasteBins");

            migrationBuilder.AddColumn<double>(
                name: "Location",
                table: "WasteBins",
                type: "float",
                nullable: true);
        }
    }
}
