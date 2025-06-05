using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcoRoute.Settings.Migrations
{
    /// <inheritdoc />
    public partial class init1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Avatars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatars", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DateFormats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FormatString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sample = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DateFormats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FontTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CssValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FontTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CultureCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ThemeColors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThemeColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DarkMode = table.Column<bool>(type: "bit", nullable: false),
                    ThemeColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnableAnimations = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LocationTracking = table.Column<bool>(type: "bit", nullable: false),
                    SessionTimeout = table.Column<int>(type: "int", nullable: false),
                    ActiveSessionLimit = table.Column<int>(type: "int", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailNotifications = table.Column<bool>(type: "bit", nullable: false),
                    SmsNotifications = table.Column<bool>(type: "bit", nullable: false),
                    PushNotifications = table.Column<bool>(type: "bit", nullable: false),
                    GoogleMapsApiKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FontTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LanguageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateFormatId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemSettings_DateFormats_DateFormatId",
                        column: x => x.DateFormatId,
                        principalTable: "DateFormats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SystemSettings_FontTypes_FontTypeId",
                        column: x => x.FontTypeId,
                        principalTable: "FontTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SystemSettings_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Avatars",
                columns: new[] { "Id", "Name", "Url" },
                values: new object[,]
                {
                    { new Guid("20101f65-5a0d-43a3-9192-f48f048cd748"), "Avatar 1", "https://api.dicebear.com/9.x/shapes/svg?seed=Can" },
                    { new Guid("4f485da0-2331-454b-98b8-3e8cf5af9fd1"), "Avatar 4", "https://api.dicebear.com/9.x/shapes/svg?seed=Deniz" },
                    { new Guid("62d4523f-3b8c-46bd-bb2e-7bc32b20b9af"), "Avatar 11", "https://api.dicebear.com/9.x/shapes/svg?seed=Zeynep" },
                    { new Guid("6dd374fc-ef36-488c-93b5-e84b0f10e3bb"), "Avatar 2", "https://api.dicebear.com/9.x/shapes/svg?seed=Olivia" },
                    { new Guid("6e9d8329-afdf-41c5-b4f1-0b132a4ae1f8"), "Avatar 15", "https://api.dicebear.com/9.x/shapes/svg?seed=Ayaz" },
                    { new Guid("72311dd4-6ee7-414c-a6ea-2db9e38aaeb3"), "Avatar 14", "https://api.dicebear.com/9.x/shapes/svg?seed=Riley" },
                    { new Guid("a8d597cf-673b-459b-8c85-561886d54cce"), "Avatar 10", "https://api.dicebear.com/9.x/shapes/svg?seed=Felix" },
                    { new Guid("ae0bda01-f887-4803-8bb5-2527b5bf2675"), "Avatar 9", "https://api.dicebear.com/9.x/shapes/svg?seed=Arda" },
                    { new Guid("bfab2c85-221b-4be2-b5e2-776249bd5d53"), "Avatar 3", "https://api.dicebear.com/9.x/shapes/svg?seed=Atlas" },
                    { new Guid("c0327eb9-af32-4f79-82b1-a60e02b67323"), "Avatar 7", "https://api.dicebear.com/9.x/shapes/svg?seed=Jasper" },
                    { new Guid("c62abf20-405e-4adc-9372-bb8b7f302e20"), "Avatar 6", "https://api.dicebear.com/9.x/shapes/svg?seed=Emir" },
                    { new Guid("d273d50a-c072-41a9-9890-a90377db7e94"), "Avatar 13", "https://api.dicebear.com/9.x/shapes/svg?seed=Selin" },
                    { new Guid("d7ea1dd4-49ee-4b20-b1dd-15b6602aada2"), "Avatar 8", "https://api.dicebear.com/9.x/shapes/svg?seed=Lina" },
                    { new Guid("e084c1bf-589e-40d8-b230-bec3cb15c62e"), "Avatar 12", "https://api.dicebear.com/9.x/shapes/svg?seed=Leo" },
                    { new Guid("f1d0748d-07f6-41d5-be33-a23856ae6e75"), "Avatar 5", "https://api.dicebear.com/9.x/shapes/svg?seed=Nova" }
                });

            migrationBuilder.InsertData(
                table: "DateFormats",
                columns: new[] { "Id", "FormatString", "Sample" },
                values: new object[,]
                {
                    { new Guid("33333333-3333-3333-3333-333333333333"), "DD/MM/YYYY", "31/12/2025" },
                    { new Guid("33333333-3333-3333-3333-333333333334"), "MM/DD/YYYY", "12/31/2025" },
                    { new Guid("33333333-3333-3333-3333-333333333335"), "YYYY-MM-DD", "2025-12-31" }
                });

            migrationBuilder.InsertData(
                table: "FontTypes",
                columns: new[] { "Id", "CssValue", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "'Open Sans', sans-serif", "Open Sans" },
                    { new Guid("11111111-1111-1111-1111-111111111112"), "Georgia, serif", "Georgia" },
                    { new Guid("11111111-1111-1111-1111-111111111113"), "'Montserrat', sans-serif", "Montserrat" },
                    { new Guid("11111111-1111-1111-1111-111111111114"), "'Playfair Display', serif", "Playfair Display" }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "CultureCode", "Name" },
                values: new object[,]
                {
                    { new Guid("22222222-2222-2222-2222-222222222222"), "tr-TR", "Türkçe" },
                    { new Guid("22222222-2222-2222-2222-222222222223"), "en-US", "English" },
                    { new Guid("22222222-2222-2222-2222-222222222224"), "de-DE", "Deutsch" }
                });

            migrationBuilder.InsertData(
                table: "ThemeColors",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { new Guid("00c75b32-92ac-472d-9ee9-6e62f1b322d4"), "Turkuaz", "#06B6D4" },
                    { new Guid("06f59e97-0966-4b25-a42b-eee16ab7b443"), "Gri", "#6B7280" },
                    { new Guid("40ff0e18-2cf0-40ed-bd84-a5f2102a8a25"), "Mor", "#8B5CF6" },
                    { new Guid("46de22db-6ac2-498d-9ac6-c163b687618d"), "Lime", "#84CC16" },
                    { new Guid("5b70c0a0-f29d-44bc-8445-f889b7e53d42"), "Teal", "#14B8A6" },
                    { new Guid("6ba60634-3355-45f6-b41c-35acab2690bd"), "Orijinal", "#2ba86d" },
                    { new Guid("8db7d2ab-469d-4102-8d46-9c7c44e96c69"), "Kırmızı", "#EF4444" },
                    { new Guid("a1feda7f-6cb1-44dc-a0c2-13ecf1a58b22"), "Sarı", "#F59E0B" },
                    { new Guid("bb5e61aa-ecd6-4c9b-8dde-e31141754128"), "Mavi", "#3B82F6" },
                    { new Guid("c111b3a5-785d-4b1a-8c5f-a9d610a7b77f"), "Pembe", "#EC4899" },
                    { new Guid("cfe022ca-f0e7-43f9-aaa0-a5607ba18381"), "Turuncu", "#F97316" },
                    { new Guid("fc01088f-e866-4c4d-8cbb-1e8f01f35f39"), "Indigo", "#6366F1" }
                });

            migrationBuilder.InsertData(
                table: "SystemSettings",
                columns: new[] { "Id", "ActiveSessionLimit", "AvatarUrl", "DarkMode", "DateFormatId", "EmailNotifications", "EnableAnimations", "FontTypeId", "GoogleMapsApiKey", "LanguageId", "LocationTracking", "PushNotifications", "SessionTimeout", "SmsNotifications", "ThemeColor", "TwoFactorEnabled", "UserId" },
                values: new object[] { new Guid("44444444-4444-4444-4444-444444444444"), 0, "https://api.dicebear.com/9.x/adventurer/svg?seed=Easton", false, new Guid("33333333-3333-3333-3333-333333333333"), true, true, new Guid("11111111-1111-1111-1111-111111111111"), "", new Guid("22222222-2222-2222-2222-222222222222"), false, true, 0, false, "#2ba86d", false, null });

            migrationBuilder.CreateIndex(
                name: "IX_SystemSettings_DateFormatId",
                table: "SystemSettings",
                column: "DateFormatId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemSettings_FontTypeId",
                table: "SystemSettings",
                column: "FontTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemSettings_LanguageId",
                table: "SystemSettings",
                column: "LanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avatars");

            migrationBuilder.DropTable(
                name: "SystemSettings");

            migrationBuilder.DropTable(
                name: "ThemeColors");

            migrationBuilder.DropTable(
                name: "DateFormats");

            migrationBuilder.DropTable(
                name: "FontTypes");

            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
