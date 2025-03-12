using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoRoute.RouteOptimization.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    MyRouteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssignedDriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StartLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalDistance = table.Column<double>(type: "float", nullable: false),
                    EstimatedFuelConsumption = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.MyRouteId);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    RouteOptimizationResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RouteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OptimizedPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalDistance = table.Column<double>(type: "float", nullable: false),
                    EstimatedTravelTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    CalculatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.RouteOptimizationResultId);
                    table.ForeignKey(
                        name: "FK_Results_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "MyRouteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Waypoints",
                columns: table => new
                {
                    WaypointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RouteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sequence = table.Column<int>(type: "int", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Waypoints", x => x.WaypointId);
                    table.ForeignKey(
                        name: "FK_Waypoints_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "MyRouteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Results_RouteId",
                table: "Results",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Waypoints_RouteId",
                table: "Waypoints",
                column: "RouteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Waypoints");

            migrationBuilder.DropTable(
                name: "Routes");
        }
    }
}
