using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EcoRoute.Settings.Migrations
{
    /// <inheritdoc />
    public partial class initialCreate : Migration
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
                    { new Guid("0b704c12-8810-4234-a4c3-e5f00cc71b68"), "Avatar 1", "https://api.dicebear.com/9.x/adventurer/svg?seed=Luis" },
                    { new Guid("1fbdddce-43e9-4d0e-94dc-d6d8281ec861"), "Avatar 4", "https://api.dicebear.com/9.x/adventurer/svg?seed=Jack" },
                    { new Guid("54933f2c-1782-4903-9859-5ea4661d529b"), "Avatar 2", "https://api.dicebear.com/9.x/adventurer/svg?seed=Avery" },
                    { new Guid("71d4bab1-e22c-45c6-a37f-80ef2d587689"), "Avatar 5", "https://api.dicebear.com/9.x/adventurer/svg?seed=Jocelyn" },
                    { new Guid("b372e881-9971-4135-abe1-2d7391268d1a"), "Avatar 6", "https://api.dicebear.com/9.x/adventurer/svg?seed=Easton" },
                    { new Guid("d8d2a28e-5195-4bd3-95b7-5755107425de"), "Avatar 3", "https://api.dicebear.com/9.x/adventurer/svg?seed=Jude" }
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
                    { new Guid("0e950196-27ed-4e8f-b3d6-5cfd40d78f5a"), "Mavi", "#3B82F6" },
                    { new Guid("3bf0fe1d-6bfb-4ba3-a2b0-4b81dc7bb6c5"), "Lime", "#84CC16" },
                    { new Guid("40c63795-23bd-436c-b2f1-3869def0d946"), "Turuncu", "#F97316" },
                    { new Guid("4296db02-aa9e-4f0f-ba05-0e3af04f9b53"), "Turkuaz", "#06B6D4" },
                    { new Guid("43a7f0bc-223b-431e-be0a-0907d56d6cc2"), "Orijinal", "#2ba86d" },
                    { new Guid("4c4ac672-af36-4ee1-bc40-dd212e667e8a"), "Kırmızı", "#EF4444" },
                    { new Guid("518f3f89-f40c-4236-857d-e968eff6533c"), "Pembe", "#EC4899" },
                    { new Guid("5c30bf77-976f-4fd5-9a52-e960d3627381"), "Teal", "#14B8A6" },
                    { new Guid("9191de29-2671-46a6-a6b3-e59d259ff8af"), "Indigo", "#6366F1" },
                    { new Guid("98843576-420d-4edd-87b9-c50a5e07d8df"), "Gri", "#6B7280" },
                    { new Guid("9abae893-5e6b-4ac9-8f0f-0daa0131b333"), "Mor", "#8B5CF6" },
                    { new Guid("f056eda2-b1af-408b-84f4-0b80e916d563"), "Sarı", "#F59E0B" }
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
