using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcoRoute.DataProcessing.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dataProcessingLogs",
                columns: table => new
                {
                    DataProcessingLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProcessName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dataProcessingLogs", x => x.DataProcessingLogId);
                });

            migrationBuilder.CreateTable(
                name: "processedDatas",
                columns: table => new
                {
                    ProcessedDataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BinId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AverageFillLevel = table.Column<double>(type: "float", nullable: false),
                    PredictedFillLevel = table.Column<double>(type: "float", nullable: false),
                    PeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processedDatas", x => x.ProcessedDataId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dataProcessingLogs");

            migrationBuilder.DropTable(
                name: "processedDatas");
        }
    }
}
