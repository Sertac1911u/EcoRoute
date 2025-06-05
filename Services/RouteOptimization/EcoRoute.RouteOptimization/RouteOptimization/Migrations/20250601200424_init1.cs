using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcoRoute.RouteOptimization.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RouteTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    VehicleId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    WasteType = table.Column<int>(type: "int", nullable: false),
                    OptimizationType = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalDistanceKm = table.Column<double>(type: "float", nullable: false),
                    EstimatedDurationMin = table.Column<int>(type: "int", nullable: false),
                    EstimatedFuelL = table.Column<double>(type: "float", nullable: false),
                    EstimatedCO2Kg = table.Column<double>(type: "float", nullable: false),
                    OverviewPolyline = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: true),
                    RouteName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Plate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RouteSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RouteTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    WasteBinId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RouteSteps_RouteTasks_RouteTaskId",
                        column: x => x.RouteTaskId,
                        principalTable: "RouteTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Description", "Plate" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "1. Araç", "59 ABC 1001" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "2. Araç", "59 DEF 1002" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "3. Araç", "59 GHI 1003" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "4. Araç", "59 JKL 1004" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "5. Araç", "59 MNO 1005" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), "6. Araç", "59 PQR 1006" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), "7. Araç", "59 STU 1007" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "8. Araç", "59 VWX 1008" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "9. Araç", "59 YZA 1009" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "10. Araç", "59 BCD 1010" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RouteSteps_RouteTaskId",
                table: "RouteSteps",
                column: "RouteTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteSteps");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "RouteTasks");
        }
    }
}
