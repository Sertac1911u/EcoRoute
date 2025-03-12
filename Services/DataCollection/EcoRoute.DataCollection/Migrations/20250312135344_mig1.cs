using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoRoute.DataCollection.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WasteBins",
                columns: table => new
                {
                    WasteBinId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<double>(type: "float", nullable: true),
                    IsFilled = table.Column<bool>(type: "bit", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeviceStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteBins", x => x.WasteBinId);
                });

            migrationBuilder.CreateTable(
                name: "BinLogs",
                columns: table => new
                {
                    BinLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WasteBinId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FillLevel = table.Column<double>(type: "float", nullable: true),
                    DeviceStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinLogs", x => x.BinLogId);
                    table.ForeignKey(
                        name: "FK_BinLogs_WasteBins_WasteBinId",
                        column: x => x.WasteBinId,
                        principalTable: "WasteBins",
                        principalColumn: "WasteBinId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProcessDatas",
                columns: table => new
                {
                    ProcessDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WasteBinId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AverageFillLevel = table.Column<double>(type: "float", nullable: true),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessDatas", x => x.ProcessDataId);
                    table.ForeignKey(
                        name: "FK_ProcessDatas_WasteBins_WasteBinId",
                        column: x => x.WasteBinId,
                        principalTable: "WasteBins",
                        principalColumn: "WasteBinId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sensors",
                columns: table => new
                {
                    SensorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WasteBinId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InstallationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sensors", x => x.SensorId);
                    table.ForeignKey(
                        name: "FK_Sensors_WasteBins_WasteBinId",
                        column: x => x.WasteBinId,
                        principalTable: "WasteBins",
                        principalColumn: "WasteBinId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnvLogs",
                columns: table => new
                {
                    EnvLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SensorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<byte>(type: "tinyint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnvLogs", x => x.EnvLogId);
                    table.ForeignKey(
                        name: "FK_EnvLogs_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "SensorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BinLogs_WasteBinId",
                table: "BinLogs",
                column: "WasteBinId");

            migrationBuilder.CreateIndex(
                name: "IX_EnvLogs_SensorId",
                table: "EnvLogs",
                column: "SensorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessDatas_WasteBinId",
                table: "ProcessDatas",
                column: "WasteBinId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_WasteBinId",
                table: "Sensors",
                column: "WasteBinId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BinLogs");

            migrationBuilder.DropTable(
                name: "EnvLogs");

            migrationBuilder.DropTable(
                name: "ProcessDatas");

            migrationBuilder.DropTable(
                name: "Sensors");

            migrationBuilder.DropTable(
                name: "WasteBins");
        }
    }
}
